Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Module AccessFunctions


    Structure StorageTable
        Dim Number As Integer
        Dim DataBlocks As List(Of Integer)
    End Structure

    Structure TableFields
        Dim Name As String
        Dim Length As Integer
        Dim Precision As Integer
        Dim Size As Integer
        Dim Offset As Integer
        Dim Type As String
        Dim CouldBeNull As Integer


    End Structure

    Structure PageParams
        Dim TableName As String
        Dim Sign As String
        Dim length As Integer
        Dim lengthLong As Long
        Dim version1 As Integer
        Dim version2 As Integer
        Dim version As Integer
        Dim PagesNum As List(Of Integer)
        Dim StorageTables As List(Of StorageTable)
        Dim Fields As List(Of TableFields)
        Dim RowSize As Integer
        Dim Records As List(Of Dictionary(Of String, Object))
        Dim BlockData As Integer
        Dim BlockBlob As Integer
        Dim TableDefinition As String
    End Structure

    Function ReadPage(reader As BinaryReader, Bytes() As Byte, Optional PageSize As Integer = 4096) As PageParams

        Dim Page = New PageParams
        Page.Sign = Encoding.UTF8.GetString(Bytes, 0, 8)
        Page.length = BitConverter.ToInt32(Bytes, 8)
        Page.version1 = BitConverter.ToInt32(Bytes, 12)
        Page.version2 = BitConverter.ToInt32(Bytes, 16)
        Page.version = BitConverter.ToInt32(Bytes, 20)

        Dim Index = 24
        Page.PagesNum = New List(Of Integer)
        Page.StorageTables = New List(Of StorageTable)

        'Получим номера страниц размещения 
        While True
            Dim blk = BitConverter.ToInt32(Bytes, Index)
            If blk = 0 Then
                Exit While
            End If
            Page.PagesNum.Add(blk)
            Index = Index + 4
            If Index > PageSize - 4 Then
                Exit While
            End If
        End While

        For Each blk In Page.PagesNum

            Dim StorageTables = New StorageTable
            StorageTables.Number = blk
            StorageTables.DataBlocks = New List(Of Integer)

            Dim bytesBlock() As Byte = New Byte(PageSize - 1) {}
            reader.BaseStream.Seek(blk * PageSize, SeekOrigin.Begin)
            reader.Read(bytesBlock, 0, PageSize)

            Dim NumberOfPages = BitConverter.ToInt32(bytesBlock, 0)

            Index = 4

            For ii = 0 To NumberOfPages - 1
                Dim dp = BitConverter.ToInt32(bytesBlock, Index)
                If dp = 0 Then
                    Exit For
                End If
                StorageTables.DataBlocks.Add(dp)
                Index = Index + 4
                If Index > PageSize - 4 Then
                    Exit For
                End If
            Next

            Page.StorageTables.Add(StorageTables)

        Next

        Return Page

    End Function

    Function ReadPage83(reader As BinaryReader, Bytes() As Byte, PageSize As Integer, TableUsersName As String) As PageParams

        Dim Page = New PageParams
        Dim PageType As Integer = BitConverter.ToInt32(Bytes, 0)
        If PageType = 64796 Then
            '0xFD1C small storage table
            '???
        ElseIf PageType = 130332 Then
            '0x01FD1C  large storage table
            '???
        End If

        'Encoding.UTF8.GetString(Bytes, 0, 8)

        'unsigned Int object_type; //0xFD1C или 0x01FD1C 
        'unsigned Int version1; 
        'unsigned Int version2; 
        'unsigned Int version3; 
        'unsigned Long int length; //64-разрядное целое! 
        'unsigned Int pages[]; 

        'Page.Sign = BitConverter.ToInt32(Bytes, 0)

        Page.version1 = BitConverter.ToInt32(Bytes, 12)
        Page.version2 = BitConverter.ToInt32(Bytes, 16)
        Page.version = BitConverter.ToInt32(Bytes, 20)
        Page.lengthLong = BitConverter.ToInt64(Bytes, 16)

        Dim Index = 24
        Page.PagesNum = New List(Of Integer)
        Page.StorageTables = New List(Of StorageTable)

        'Получим номера страниц размещения 
        While True
            Dim blk = BitConverter.ToInt32(Bytes, Index)
            If blk = 0 Then
                Exit While
            End If
            Page.PagesNum.Add(blk)
            Index = Index + 4
            If Index > 4092 Then 'TODO!
                Exit While
            End If
        End While

        Dim PagesCountTableStructure = Page.PagesNum.Count
        Dim BytesTableStructure() As Byte = New Byte(PagesCountTableStructure * PageSize - 1) {}

        Dim BytesTableStructureBlockNumbers() As Byte = New Byte(PagesCountTableStructure * PageSize - 1) {}


        Dim i = 0
        For Each blk In Page.PagesNum
            Dim bytesBlock() As Byte = New Byte(PageSize - 1) {}
            reader.BaseStream.Seek(blk * PageSize, SeekOrigin.Begin)
            reader.Read(bytesBlock, 0, PageSize)
            For a = 0 To PageSize - 1
                BytesTableStructure(i + a) = bytesBlock(a)
            Next
            i = i + PageSize
        Next

        'Dim TotalBlocks = BitConverter.ToInt32(BytesTableStructure, 294)
        i = 256
        Dim Pos = 0
        While 1 = 1
            Dim NextBlock = BitConverter.ToInt32(BytesTableStructure, i)

            For j = 7 To 256
                BytesTableStructureBlockNumbers(Pos) = BytesTableStructure(j + i - 1)
                Pos = Pos + 1
            Next

            If NextBlock = 0 Then
                Exit While
            End If

            i = i + 256

        End While

        Dim TotalBlocks = BitConverter.ToInt32(BytesTableStructureBlockNumbers, 32)
        Dim PagesWithTableSchema = New List(Of Integer)

        For j = 1 To TotalBlocks

            Dim BlockNumber = BitConverter.ToInt32(BytesTableStructureBlockNumbers, 32 + j * 4)
            PagesWithTableSchema.Add(BlockNumber)
            ' Dim aaa = 0

        Next

        For Each TablePageNumber In PagesWithTableSchema

            Dim Position = TablePageNumber * 256

            Dim NextBlock = BitConverter.ToInt32(BytesTableStructure, Position)
            Dim StringLen = BitConverter.ToInt16(BytesTableStructure, Position + 4)

            Dim StrDefinition = Encoding.UTF8.GetString(BytesTableStructure, Position + 6, StringLen)

            While NextBlock > 0

                Position = NextBlock * 256

                NextBlock = BitConverter.ToInt32(BytesTableStructure, Position)
                StringLen = BitConverter.ToInt16(BytesTableStructure, Position + 4)

                StrDefinition = StrDefinition + Encoding.UTF8.GetString(BytesTableStructure, Position + 6, StringLen)


            End While

            Dim TableDefinition = ParserServices.ParsesClass.ParseString(StrDefinition)
            If TableDefinition(0)(0).ToString.ToUpper = """" + TableUsersName + """" Then
                Page.TableDefinition = StrDefinition
            End If


        Next

        Return Page

    End Function

    Function ReadInfoBase(FileName As String, TableNameUsers As String, ByRef DatabaseVersion As String) As PageParams

        Dim fs As New FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim reader As New BinaryReader(fs)
        'Dim bytes() As Byte = New Byte(fs.Length) {}

        Dim bytesBlock() As Byte = New Byte(4096 - 1) {}
        reader.Read(bytesBlock, 0, 4096)

        Dim Str = Encoding.UTF8.GetString(bytesBlock, 0, 8)
        Dim V1 = bytesBlock(8).ToString
        Dim V2 = bytesBlock(9).ToString
        Dim V3 = bytesBlock(10).ToString
        DatabaseVersion = V1 + "." + V2 + "." + V3

        Dim DBSize = BitConverter.ToInt32(bytesBlock, 12)
        Dim PageSize = BitConverter.ToInt32(bytesBlock, 20)

        If PageSize > 4096 Then
            reader.BaseStream.Seek(PageSize, SeekOrigin.Begin)
        End If

        If DatabaseVersion.StartsWith("8.3") Then

            bytesBlock = New Byte(PageSize - 1) {}

            'второй блок пропускаем
            reader.Read(bytesBlock, 0, PageSize)

            'корневой блок
            reader.Read(bytesBlock, 0, PageSize)

            Dim Param = ReadPage83(reader, bytesBlock, PageSize, TableNameUsers)

            ReadDataFromTable83(reader, Param, PageSize)



        Else
            'второй блок пропускаем
            reader.Read(bytesBlock, 0, 4096)

            'корневой блок
            reader.Read(bytesBlock, 0, 4096)

            Dim Param = ReadPage(reader, bytesBlock)

            Dim Language = ""
            Dim NumberOfTables = 0
            Dim HeaderTables As List(Of Integer) = New List(Of Integer)

            Dim i = 0
            For Each ST In Param.StorageTables

                Dim bytesStorageTables() As Byte = New Byte(4096 * ST.DataBlocks.Count - 1) {}

                For Each DB In ST.DataBlocks
                    Dim TempBlock() As Byte = New Byte(4095) {}
                    reader.BaseStream.Seek(DB * 4096, SeekOrigin.Begin)
                    reader.Read(TempBlock, 0, 4096)
                    For Each ElemByte In TempBlock
                        bytesStorageTables(i) = ElemByte
                        i = i + 1
                    Next
                Next

                Language = Encoding.UTF8.GetString(bytesStorageTables, 0, 32)
                NumberOfTables = BitConverter.ToInt32(bytesStorageTables, 32)

                For i = 0 To NumberOfTables - 1
                    Dim PageNum = BitConverter.ToInt32(bytesStorageTables, 36 + i * 4)
                    HeaderTables.Add(PageNum)
                Next

            Next

            'прочитаем первые страницы таблиц
            For Each HT In HeaderTables



                reader.BaseStream.Seek(HT * 4096, SeekOrigin.Begin)
                reader.Read(bytesBlock, 0, 4096)

                Dim PageHeader = ReadPage(reader, bytesBlock)
                PageHeader.Fields = New List(Of TableFields)

                For Each ST In PageHeader.StorageTables
                    For Each DB In ST.DataBlocks

                        ReadDataFromTable(reader, DB, bytesBlock, PageHeader, TableNameUsers)

                        If PageHeader.TableName = TableNameUsers Then
                            reader.Close()
                            Return PageHeader
                        End If
                    Next
                Next

            Next
        End If



        reader.Close()
        Return Nothing

    End Function

    Sub WritePasswordIntoInfoBaseRepo(FileName As String, PageHeader As PageParams, UserID As Byte(), NewPass As String, Offset As Integer)

        Dim fs As New FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Write)
        Dim reader As New BinaryReader(fs)

        Dim bytesBlock1() As Byte = New Byte(4096 - 1) {}
        reader.BaseStream.Seek(PageHeader.BlockData * 4096, SeekOrigin.Begin)
        reader.Read(bytesBlock1, 0, 4096)

        Dim DataPage = ReadPage(reader, bytesBlock1)

        Dim TotalBlocks = 0
        For Each ST In DataPage.StorageTables
            TotalBlocks = TotalBlocks + ST.DataBlocks.Count
        Next

        Dim bytesBlock() As Byte = New Byte(4096 * TotalBlocks - 1) {}

        Dim i = 0
        For Each ST In DataPage.StorageTables

            For Each DB In ST.DataBlocks
                Dim TempBlock() As Byte = New Byte(4095) {}
                reader.BaseStream.Seek(DB * 4096, SeekOrigin.Begin)
                reader.Read(TempBlock, 0, 4096)
                For Each ElemByte In TempBlock
                    bytesBlock(i) = ElemByte
                    i = i + 1
                Next
            Next
        Next
        reader.Close()

        Dim Test = Encoding.Unicode.GetString(bytesBlock, Offset, 64)
        Dim Pass = Encoding.Unicode.GetBytes(NewPass)

        For i = 0 To Pass.Length - 1
            bytesBlock(i + Offset) = Pass(i)
        Next

        fs = New FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Write)
        Dim writer As New BinaryWriter(fs)

        i = 0
        For Each ST In DataPage.StorageTables
            For Each DB In ST.DataBlocks
                Dim TempBlock() As Byte = New Byte(4095) {}
                For j = 0 To 4095
                    TempBlock(j) = bytesBlock(i)
                    i = i + 1
                Next

                writer.Seek(DB * 4096, SeekOrigin.Begin)
                writer.Write(TempBlock)

            Next
        Next


        writer.Close()

    End Sub

    Sub WritePasswordIntoInfoBaseIB(FileName As String, PageHeader As PageParams, UserID As Byte(), NewData As Byte(), DataPos As Integer, DataSize As Integer)

        Dim fs As New FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Write)
        Dim reader As New BinaryReader(fs)

        Dim bytesBlock1() As Byte = New Byte(4096 - 1) {}
        reader.BaseStream.Seek(PageHeader.BlockBlob * 4096, SeekOrigin.Begin)
        reader.Read(bytesBlock1, 0, 4096)

        Dim DataPage = ReadPage(reader, bytesBlock1)

        Dim TotalBlocks = 0
        For Each ST In DataPage.StorageTables
            TotalBlocks = TotalBlocks + ST.DataBlocks.Count
        Next

        Dim bytesBlock() As Byte = New Byte(4096 * TotalBlocks - 1) {}

        Dim i = 0
        For Each ST In DataPage.StorageTables

            For Each DB In ST.DataBlocks
                Dim TempBlock() As Byte = New Byte(4095) {}
                reader.BaseStream.Seek(DB * 4096, SeekOrigin.Begin)
                reader.Read(TempBlock, 0, 4096)
                For Each ElemByte In TempBlock
                    bytesBlock(i) = ElemByte
                    i = i + 1
                Next
            Next
        Next
        reader.Close()



        Dim NextBlock = DataPos
        Dim Pos = DataPos * 256
        'Dim ByteBlock() As Byte = New Byte(DataSize - 1) {}
        i = 0
        While NextBlock > 0

            NextBlock = BitConverter.ToInt32(bytesBlock, Pos)
            Dim BlockSize = BitConverter.ToInt16(bytesBlock, Pos + 4)

            For j = 0 To BlockSize - 1
                bytesBlock(Pos + 6 + j) = NewData(i)
                i = i + 1
            Next
            Pos = NextBlock * 256
        End While

        'Return ByteBlock



        'Dim Test = Encoding.Unicode.GetString(bytesBlock, Offset, 64)
        'Dim Pass = Encoding.Unicode.GetBytes(NewPass)

        'For i = 0 To Pass.Length - 1
        '    bytesBlock(i + Offset) = Pass(i)
        'Next

        fs = New FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Write)
        Dim writer As New BinaryWriter(fs)

        i = 0
        For Each ST In DataPage.StorageTables
            For Each DB In ST.DataBlocks
                Dim TempBlock() As Byte = New Byte(4095) {}
                For j = 0 To 4095
                    TempBlock(j) = bytesBlock(i)
                    i = i + 1
                Next

                writer.Seek(DB * 4096, SeekOrigin.Begin)
                writer.Write(TempBlock)

            Next
        Next


        writer.Close()

    End Sub

    Sub ReadDataFromTable83(reader As BinaryReader, ByRef PageHeader As PageParams, PageSize As Integer)

        Dim ParsedString = ParserServices.ParsesClass.ParseString(PageHeader.TableDefinition)

        PageHeader.Fields = New List(Of TableFields)

        Dim RowSize = 1

        Dim TableName = ParsedString(0)(0).ToString.Replace("""", "").ToUpper

        PageHeader.TableName = TableName

        For Each a In ParsedString(0)(2)
            If TypeOf a Is String Then
                Continue For
            End If

            Dim Field = New TableFields
            Field.Name = a(0).ToString.Replace("""", "")
            Field.Type = a(1).ToString.Replace("""", "")
            Field.CouldBeNull = a(2)
            Field.Length = a(3)
            Field.Precision = a(4)


            Dim FieldSize = Field.CouldBeNull

            If Field.Type = "B" Then
                FieldSize = FieldSize + Field.Length
            ElseIf Field.Type = "L" Then
                FieldSize = FieldSize + 1
            ElseIf Field.Type = "N" Then
                FieldSize = FieldSize + Math.Truncate((Field.Length + 2) / 2)
            ElseIf Field.Type = "NC" Then
                FieldSize = FieldSize + Field.Length * 2
            ElseIf Field.Type = "NVC" Then
                FieldSize = FieldSize + Field.Length * 2 + 2
            ElseIf Field.Type = "RV" Then
                FieldSize = FieldSize + 16
            ElseIf Field.Type = "I" Then
                FieldSize = FieldSize + 8
            ElseIf Field.Type = "T" Then
                FieldSize = FieldSize + 8
            ElseIf Field.Type = "DT" Then
                FieldSize = FieldSize + 7
            ElseIf Field.Type = "NT" Then
                FieldSize = FieldSize + 8
            End If

            Field.Size = FieldSize
            Field.Offset = RowSize

            RowSize = RowSize + FieldSize

            PageHeader.Fields.Add(Field)

        Next



        PageHeader.RowSize = RowSize

        '{"Files",118,119,96}
        'Данные, BLOB, индексы

        Dim BlockData = Convert.ToInt32(ParsedString(0)(5)(1))
        Dim BlockBlob = Convert.ToInt32(ParsedString(0)(5)(2))

        PageHeader.BlockData = BlockData
        PageHeader.BlockBlob = BlockBlob

        ReadDataPage83(PageHeader, TableName, BlockData, BlockBlob, reader, PageSize)

    End Sub

    Sub ReadDataPage83(ByRef PageHeader As PageParams, table As String, block As Integer, BlockBlob As Integer, reader As BinaryReader, PageSize As Integer)

        PageHeader.Records = New List(Of Dictionary(Of String, Object))

        Dim bytesBlock1() As Byte = New Byte(PageSize - 1) {}
        reader.BaseStream.Seek(block * PageSize, SeekOrigin.Begin)
        reader.Read(bytesBlock1, 0, PageSize)

        Dim DataPage = ReadPage(reader, bytesBlock1, PageSize)

        Dim TotalBlocks = 0
        For Each ST In DataPage.StorageTables
            TotalBlocks = TotalBlocks + ST.DataBlocks.Count
        Next

        Dim bytesBlock() As Byte = New Byte(PageSize * TotalBlocks - 1) {}

        Dim i = 0
        For Each ST In DataPage.StorageTables

            For Each DB In ST.DataBlocks
                Dim TempBlock() As Byte = New Byte(PageSize - 1) {}
                reader.BaseStream.Seek(DB * PageSize, SeekOrigin.Begin)
                reader.Read(TempBlock, 0, PageSize)
                For Each ElemByte In TempBlock
                    bytesBlock(i) = ElemByte
                    i = i + 1
                Next
            Next
        Next

        Dim Size = DataPage.length / PageHeader.RowSize

        For i = 1 To Size - 1

            Dim Pos = PageHeader.RowSize * i

            Dim FieldStartPos = 0

            Dim IsDeleted = BitConverter.ToBoolean(bytesBlock, Pos)

            Dim Dict = New Dictionary(Of String, Object)
            Dict.Add("IsDeleted", IsDeleted)

            For Each Field In PageHeader.Fields

                Dim Pos1 = Pos + 1 + FieldStartPos

                If Field.Name = "PASSWORD" Then
                    Dict.Add("OFFSET_PASSWORD", Pos1)
                End If
                If Field.Name = "DATA" Then
                    Dict.Add("DATA_POS", BitConverter.ToInt32(bytesBlock, Pos1))
                    Dict.Add("DATA_SIZE", BitConverter.ToInt32(bytesBlock, Pos1 + 4))
                End If

                Dim BytesVal = Nothing

                If Field.Type = "B" Then

                    Dim Strguid = Convert.ToBase64String(bytesBlock, Pos1 + Field.CouldBeNull, Field.Size - Field.CouldBeNull)

                    BytesVal = Convert.FromBase64String(Strguid)

                    'Dim G = Convert.

                ElseIf Field.Type = "L" Then

                    BytesVal = BitConverter.ToBoolean(bytesBlock, Pos1 + Field.CouldBeNull)

                ElseIf Field.Type = "DT" Then

                    Dim BytesDate(6) As Byte ' 7 байт
                    For AA = 0 To 6
                        BytesDate(AA) = Convert.ToString(bytesBlock(Pos1 + AA), 16)
                    Next

                    Try
                        BytesVal = New DateTime(BytesDate(0) * 100 + BytesDate(1),
                                                                          BytesDate(2),
                                                                          BytesDate(3),
                                                                          BytesDate(4),
                                                                          BytesDate(5),
                                                                          BytesDate(6))
                    Catch ex As Exception
                        BytesVal = ""
                    End Try


                ElseIf Field.Type = "I" Then
                    'двоичные данные неограниченной длины
                    'в рамках хранилища 8.3.6 их быть не должно


                    Dim DataPos = BitConverter.ToInt32(bytesBlock, Pos1)
                    Dim DataSize = BitConverter.ToInt32(bytesBlock, Pos1 + 4)

                    Dim BytesValTemp = GetBlodData(BlockBlob, DataPos, DataSize, reader)

                    Dim DataKey() As Byte = New Byte(0) {}
                    Dim DataKeySize As Integer = 0

                    BytesVal = DecodePasswordStructure(BytesValTemp, DataKeySize, DataKey)

                    Dict.Add("DATA_KEYSIZE", DataKeySize)
                    Dict.Add("DATA_KEY", DataKey)

                ElseIf Field.Type = "NT" Then
                    'Строка неограниченной длины
                    BytesVal = "" 'TODO
                ElseIf Field.Type = "N" Then
                    'число
                    BytesVal = 0

                    Dim StrNumber = ""
                    For AA = 0 To Field.Size - 1
                        Dim character = Convert.ToString(bytesBlock(Pos1 + AA), 16)
                        StrNumber = StrNumber + IIf(character.Length = 1, "0", "") + character
                    Next

                    Dim FirstSimbol = StrNumber.Substring(0, 1)

                    StrNumber = StrNumber.Substring(1, Field.Length)

                    If String.IsNullOrEmpty(StrNumber) Then
                        BytesVal = 0
                    Else

                        BytesVal = Convert.ToInt32(StrNumber) / IIf(Field.Precision > 0, (Field.Precision * 10), 1)

                        If FirstSimbol = "0" Then
                            BytesVal = BytesVal * (-1)
                        End If
                    End If

                ElseIf Field.Type = "NVC" Then
                    'Строка переменной длины
                    Dim BytesStr(1) As Byte
                    For AA = 0 To 1
                        BytesStr(AA) = bytesBlock(Pos1 + AA + Field.CouldBeNull)
                    Next

                    Dim L = Math.Min(Field.Size, (BytesStr(0) + BytesStr(1) * 256) * 2)

                    BytesVal = Encoding.Unicode.GetString(bytesBlock, Pos1 + 2 + Field.CouldBeNull, L).Trim ' was L- 2

                ElseIf Field.Type = "NC" Then
                    'строка фиксированной длины
                    BytesVal = Encoding.Unicode.GetString(bytesBlock, Pos1, Field.Size)

                End If

                Dict.Add(Field.Name, BytesVal)

                FieldStartPos = FieldStartPos + Field.Size

            Next

            PageHeader.Records.Add(Dict)

        Next

    End Sub

    Sub ReadDataFromTable(reader As BinaryReader, DB As Integer, bytesBlock() As Byte, ByRef PageHeader As PageParams, TableNameUsers As String)

        reader.BaseStream.Seek(DB * 4096, SeekOrigin.Begin)
        reader.Read(bytesBlock, 0, 4096)

        Dim TableDescr = ""
        For i = 0 To Math.Min(PageHeader.length - 1, 4096 / 2 - 1)
            TableDescr = TableDescr + Encoding.UTF8.GetString(bytesBlock, i * 2, 1)
        Next

        Dim ParsedString = ParserServices.ParsesClass.ParseString(TableDescr)

        Dim RowSize = 1

        Dim TableName = ParsedString(0)(0).ToString.Replace("""", "").ToUpper

        PageHeader.TableName = TableName

        If Not TableName = TableNameUsers Then
            Exit Sub
        End If

        For Each a In ParsedString(0)(2)
            If TypeOf a Is String Then
                Continue For
            End If

            Dim Field = New TableFields
            Field.Name = a(0).ToString.Replace("""", "")
            Field.Type = a(1).ToString.Replace("""", "")
            Field.CouldBeNull = a(2)
            Field.Length = a(3)
            Field.Precision = a(4)


            Dim FieldSize = Field.CouldBeNull

            If Field.Type = "B" Then
                FieldSize = FieldSize + Field.Length
            ElseIf Field.Type = "L"
                FieldSize = FieldSize + 1
            ElseIf Field.Type = "N"
                FieldSize = FieldSize + Math.Truncate((Field.Length + 2) / 2)
            ElseIf Field.Type = "NC"
                FieldSize = FieldSize + Field.Length * 2
            ElseIf Field.Type = "NVC"
                FieldSize = FieldSize + Field.Length * 2 + 2
            ElseIf Field.Type = "RV"
                FieldSize = FieldSize + 16
            ElseIf Field.Type = "I"
                FieldSize = FieldSize + 8
            ElseIf Field.Type = "T"
                FieldSize = FieldSize + 8
            ElseIf Field.Type = "DT"
                FieldSize = FieldSize + 7
            ElseIf Field.Type = "NT"
                FieldSize = FieldSize + 8
            End If

            Field.Size = FieldSize
            Field.Offset = RowSize

            RowSize = RowSize + FieldSize

            PageHeader.Fields.Add(Field)

        Next



        PageHeader.RowSize = RowSize

        '{"Files",118,119,96}
        'Данные, BLOB, индексы

        Dim BlockData = Convert.ToInt32(ParsedString(0)(5)(1))
        Dim BlockBlob = Convert.ToInt32(ParsedString(0)(5)(2))

        PageHeader.BlockData = BlockData
        PageHeader.BlockBlob = BlockBlob

        ReadDataPage(PageHeader, TableName, BlockData, BlockBlob, reader)



    End Sub

    Function GetBlodData(BlockBlob As Integer, Dataindex As Integer, Datasize As Integer, reader As BinaryReader) As Byte()

        Dim bytesBlock1() As Byte = New Byte(4096 - 1) {}
        reader.BaseStream.Seek(BlockBlob * 4096, SeekOrigin.Begin)
        reader.Read(bytesBlock1, 0, 4096)

        Dim DataPage = ReadPage(reader, bytesBlock1)

        Dim TotalBlocks = 0
        For Each ST In DataPage.StorageTables
            TotalBlocks = TotalBlocks + ST.DataBlocks.Count
        Next

        Dim bytesBlock() As Byte = New Byte(4096 * TotalBlocks - 1) {}

        Dim i = 0
        For Each ST In DataPage.StorageTables

            For Each DB In ST.DataBlocks
                Dim TempBlock() As Byte = New Byte(4095) {}
                reader.BaseStream.Seek(DB * 4096, SeekOrigin.Begin)
                reader.Read(TempBlock, 0, 4096)
                For Each ElemByte In TempBlock
                    bytesBlock(i) = ElemByte
                    i = i + 1
                Next
            Next
        Next

        Dim NextBlock = Dataindex
        Dim Pos = Dataindex * 256
        Dim ByteBlock() As Byte = New Byte(Datasize - 1) {}
        i = 0
        While NextBlock > 0

            NextBlock = BitConverter.ToInt32(bytesBlock, Pos)
            Dim BlockSize = BitConverter.ToInt16(bytesBlock, Pos + 4)

            'Dim ByteTemp() As Byte = New Byte(BlockSize - 1) {}

            For j = 0 To BlockSize - 1
                ByteBlock(i) = bytesBlock(Pos + 6 + j)
                i = i + 1
            Next

            Pos = NextBlock * 256

        End While

        Return ByteBlock

    End Function

    Sub ReadDataPage(ByRef PageHeader As PageParams, table As String, block As Integer, BlockBlob As Integer, reader As BinaryReader)

        PageHeader.Records = New List(Of Dictionary(Of String, Object))

        Dim bytesBlock1() As Byte = New Byte(4096 - 1) {}
        reader.BaseStream.Seek(block * 4096, SeekOrigin.Begin)
        reader.Read(bytesBlock1, 0, 4096)

        Dim DataPage = ReadPage(reader, bytesBlock1)

        Dim TotalBlocks = 0
        For Each ST In DataPage.StorageTables
            TotalBlocks = TotalBlocks + ST.DataBlocks.Count
        Next

        Dim bytesBlock() As Byte = New Byte(4096 * TotalBlocks - 1) {}

        Dim i = 0
        For Each ST In DataPage.StorageTables

            For Each DB In ST.DataBlocks
                Dim TempBlock() As Byte = New Byte(4095) {}
                reader.BaseStream.Seek(DB * 4096, SeekOrigin.Begin)
                reader.Read(TempBlock, 0, 4096)
                For Each ElemByte In TempBlock
                    bytesBlock(i) = ElemByte
                    i = i + 1
                Next
            Next
        Next

        Dim Size = DataPage.length / PageHeader.RowSize

        For i = 1 To Size - 1

            Dim Pos = PageHeader.RowSize * i

            Dim FieldStartPos = 0

            Dim IsDeleted = BitConverter.ToBoolean(bytesBlock, Pos)

            Dim Dict = New Dictionary(Of String, Object)
            Dict.Add("IsDeleted", IsDeleted)

            For Each Field In PageHeader.Fields

                Dim Pos1 = Pos + 1 + FieldStartPos

                If Field.Name = "PASSWORD" Then
                    Dict.Add("OFFSET_PASSWORD", Pos1)
                End If
                If Field.Name = "DATA" Then
                    Dict.Add("DATA_POS", BitConverter.ToInt32(bytesBlock, Pos1))
                    Dict.Add("DATA_SIZE", BitConverter.ToInt32(bytesBlock, Pos1 + 4))
                End If

                Dim BytesVal = Nothing

                If Field.Type = "B" Then

                    Dim Strguid = Convert.ToBase64String(bytesBlock, Pos1 + Field.CouldBeNull, Field.Size - Field.CouldBeNull)

                    BytesVal = Convert.FromBase64String(Strguid)

                    'Dim G = Convert.

                ElseIf Field.Type = "L" Then

                    BytesVal = BitConverter.ToBoolean(bytesBlock, Pos1 + Field.CouldBeNull)

                ElseIf Field.Type = "DT" Then

                    Dim BytesDate(6) As Byte ' 7 байт
                    For AA = 0 To 6
                        BytesDate(AA) = Convert.ToString(bytesBlock(Pos1 + AA), 16)
                    Next

                    Try
                        BytesVal = New DateTime(BytesDate(0) * 100 + BytesDate(1),
                                                                          BytesDate(2),
                                                                          BytesDate(3),
                                                                          BytesDate(4),
                                                                          BytesDate(5),
                                                                          BytesDate(6))
                    Catch ex As Exception
                        BytesVal = ""
                    End Try


                ElseIf Field.Type = "I" Then
                    'двоичные данные неограниченной длины
                    'в рамках хранилища 8.3.6 их быть не должно


                    Dim DataPos = BitConverter.ToInt32(bytesBlock, Pos1)
                    Dim DataSize = BitConverter.ToInt32(bytesBlock, Pos1 + 4)

                    Dim BytesValTemp = GetBlodData(BlockBlob, DataPos, DataSize, reader)

                    Dim DataKey() As Byte = New Byte(0) {}
                    Dim DataKeySize As Integer = 0

                    BytesVal = DecodePasswordStructure(BytesValTemp, DataKeySize, DataKey)

                    Dict.Add("DATA_KEYSIZE", DataKeySize)
                    Dict.Add("DATA_KEY", DataKey)

                ElseIf Field.Type = "NT" Then
                    'Строка неограниченной длины
                    BytesVal = "" 'TODO
                ElseIf Field.Type = "N" Then
                    'число
                    BytesVal = 0

                    Dim StrNumber = ""
                    For AA = 0 To Field.Size - 1
                        Dim character = Convert.ToString(bytesBlock(Pos1 + AA), 16)
                        StrNumber = StrNumber + IIf(character.Length = 1, "0", "") + character
                    Next

                    Dim FirstSimbol = StrNumber.Substring(0, 1)

                    StrNumber = StrNumber.Substring(1, Field.Length)

                    If String.IsNullOrEmpty(StrNumber) Then
                        BytesVal = 0
                    Else

                        BytesVal = Convert.ToInt32(StrNumber) / IIf(Field.Precision > 0, (Field.Precision * 10), 1)

                        If FirstSimbol = "0" Then
                            BytesVal = BytesVal * (-1)
                        End If
                    End If

                ElseIf Field.Type = "NVC" Then
                    'Строка переменной длины
                    Dim BytesStr(1) As Byte
                    For AA = 0 To 1
                        BytesStr(AA) = bytesBlock(Pos1 + AA + Field.CouldBeNull)
                    Next

                    Dim L = Math.Min(Field.Size, (BytesStr(0) + BytesStr(1) * 256) * 2)

                    BytesVal = Encoding.Unicode.GetString(bytesBlock, Pos1 + 2 + Field.CouldBeNull, L).Trim ' was L- 2

                ElseIf Field.Type = "NC" Then
                    'строка фиксированной длины
                    BytesVal = Encoding.Unicode.GetString(bytesBlock, Pos1, Field.Size)

                End If

                Dict.Add(Field.Name, BytesVal)

                FieldStartPos = FieldStartPos + Field.Size

            Next

            PageHeader.Records.Add(Dict)

        Next

    End Sub

    Function DecodePasswordStructure(bytes_Input As Byte(), ByRef KeySize As Integer, ByRef KeyData As Byte()) As String

        Dim Base = Convert.ToInt16(bytes_Input(0), 10)

        KeySize = Base
        KeyData = New Byte(Base - 1) {}
        For a = 1 To Base
            KeyData(a - 1) = bytes_Input(a)
        Next

        Dim i = Base + 1
        Dim j = 1
        Dim MaxI = bytes_Input.Length

        Dim BytesResult() As Byte = New Byte(MaxI - Base - 1) {}

        While i < MaxI
            If j > Base Then
                j = 1
            End If

            Dim AA = Convert.ToInt16(bytes_Input(i), 10)
            Dim BB = Convert.ToInt16(bytes_Input(j), 10)
            Dim CC = AA Xor BB ' 239 for first

            BytesResult(i - Base - 1) = CC

            i = i + 1
            j = j + 1

        End While

        Return Encoding.UTF8.GetString(BytesResult)

    End Function

    Function EncodePasswordStructure(Str As String, ByVal KeySize As Integer, ByVal KeyData As Byte()) As Byte()

        Dim bytes_Input = Encoding.UTF8.GetBytes(Str)

        Dim Base = KeySize

        Dim BytesResult() As Byte = New Byte(bytes_Input.Length + Base - 1) {}
        BytesResult(0) = Base
        For ii = 1 To Base
            BytesResult(ii) = KeyData(ii - 1)
        Next

        Dim MaxI = bytes_Input.Length - 1
        Dim i = 1
        Dim j = 1

        While i <= MaxI
            If j > Base Then
                j = 1
            End If

            Dim AA = Convert.ToInt16(bytes_Input(i - 1), 10)
            Dim BB = Convert.ToInt16(BytesResult(j), 10)

            BytesResult(i + Base) = AA Xor BB

            i = i + 1
            j = j + 1

        End While

        Return BytesResult

    End Function

    Public Function EncryptStringSHA1(ByVal Str As String) As String

        Dim sha As New SHA1CryptoServiceProvider ' declare sha as a new SHA1CryptoServiceProvider
        Dim bytesToHash() As Byte ' and here is a byte variable

        bytesToHash = System.Text.Encoding.UTF8.GetBytes(Str) ' covert the password into ASCII code

        bytesToHash = sha.ComputeHash(bytesToHash) ' this is where the magic starts and the encryption begins

        Return Convert.ToBase64String(bytesToHash)

        Dim result As String = ""

        For Each b As Byte In bytesToHash
            result += b.ToString("x2")
        Next

        Return result

    End Function


End Module

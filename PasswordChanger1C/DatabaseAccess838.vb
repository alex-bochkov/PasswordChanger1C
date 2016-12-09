Imports System.IO
Imports System.Text

Module DatabaseAccess838

    Function ReadInfoBase(reader As BinaryReader, TableNameUsers As String, PageSize As Integer) As PageParams

        Dim bytesBlock = New Byte(PageSize - 1) {}

        'второй блок пропускаем
        reader.Read(bytesBlock, 0, PageSize)

        'корневой блок
        reader.Read(bytesBlock, 0, PageSize)

        Dim Param = ReadPage83(reader, bytesBlock, PageSize, TableNameUsers)
        Param.PageSize = PageSize

        CommonModule.ParseTableDefinition(Param)

        ReadDataPage83(Param, Param.TableName, Param.BlockData, Param.BlockBlob, reader, PageSize)

        Return Param

    End Function

    Function ReadPage83(reader As BinaryReader, Bytes() As Byte, PageSize As Integer, TableUsersName As String) As PageParams

        Dim Page As PageParams = ReadObjectPageDefinition(reader, Bytes, PageSize)
        Page.BinaryData = ReadAllStoragePagesForObject(reader, Page)

        Dim PagesCountTableStructure = Page.PagesNum.Count
        Dim BytesTableStructure() As Byte = Page.BinaryData

        Dim BytesTableStructureBlockNumbers() As Byte = New Byte(PagesCountTableStructure * PageSize - 1) {}

        Dim i = 256
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

            i = NextBlock * 256

        End While

        Dim TotalBlocks = BitConverter.ToInt32(BytesTableStructureBlockNumbers, 32)
        Dim PagesWithTableSchema = New List(Of Integer)

        For j = 1 To TotalBlocks

            Dim BlockNumber = BitConverter.ToInt32(BytesTableStructureBlockNumbers, 32 + j * 4)
            PagesWithTableSchema.Add(BlockNumber)

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
                Exit For
            End If
        Next

        Return Page

    End Function

    Sub ReadDataPage83(ByRef PageHeader As PageParams, table As String, block As Integer, BlockBlob As Integer, reader As BinaryReader, PageSize As Integer)

        PageHeader.Records = New List(Of Dictionary(Of String, Object))

        Dim bytesBlock1() As Byte = New Byte(PageSize - 1) {}
        reader.BaseStream.Seek(block * PageSize, SeekOrigin.Begin)
        reader.Read(bytesBlock1, 0, PageSize)

        Dim DataPage As PageParams = ReadObjectPageDefinition(reader, bytesBlock1, PageSize)
        DataPage.BinaryData = ReadAllStoragePagesForObject(reader, DataPage)

        Dim bytesBlock() As Byte = DataPage.BinaryData

        Dim Size = DataPage.Length / PageHeader.RowSize

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

                    If Field.Name = "DATA" Then
                        Dict.Add("DATA_POS", DataPos)
                        Dict.Add("DATA_SIZE", DataSize)
                    End If

                    Dim BytesValTemp = GetBlodData83(BlockBlob, DataPos, DataSize, reader, PageSize)

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


    Function GetBlodData83(BlockBlob As Integer, Dataindex As Integer, Datasize As Integer, reader As BinaryReader, PageSize As Integer) As Byte()

        Dim bytesBlock1() As Byte = New Byte(PageSize - 1) {}
        reader.BaseStream.Seek(BlockBlob * PageSize, SeekOrigin.Begin)
        reader.Read(bytesBlock1, 0, PageSize)

        Dim DataPage As PageParams = ReadObjectPageDefinition(reader, bytesBlock1, PageSize)
        DataPage.BinaryData = ReadAllStoragePagesForObject(reader, DataPage)

        Dim bytesBlock() As Byte = DataPage.BinaryData

        Dim i = 0

        Dim NextBlock = Dataindex
        Dim Pos = Dataindex * 256
        Dim ByteBlock() As Byte = New Byte(Datasize - 1) {}
        i = 0
        While NextBlock > 0

            NextBlock = BitConverter.ToInt32(bytesBlock, Pos)
            Dim BlockSize = BitConverter.ToInt16(bytesBlock, Pos + 4)

            For j = 0 To BlockSize - 1
                ByteBlock(i) = bytesBlock(Pos + 6 + j)
                i = i + 1
            Next

            Pos = NextBlock * 256

        End While

        Return ByteBlock

    End Function


    Private Function ReadAllStoragePagesForObject(reader As BinaryReader, Page As PageParams) As Byte()

        Dim PagesCountTableStructure = Page.PagesNum.Count
        Dim BytesTableStructure() As Byte = New Byte(PagesCountTableStructure * Page.PageSize - 1) {}

        Dim i = 0
        For Each blk In Page.PagesNum
            Dim bytesBlock() As Byte = New Byte(Page.PageSize - 1) {}
            reader.BaseStream.Seek(blk * Page.PageSize, SeekOrigin.Begin)
            reader.Read(bytesBlock, 0, Page.PageSize)
            For a = 0 To Page.PageSize - 1
                BytesTableStructure(i + a) = bytesBlock(a)
            Next
            i = i + Page.PageSize
        Next

        Return BytesTableStructure

    End Function

    Function ReadObjectPageDefinition(reader As BinaryReader, Bytes() As Byte, Optional PageSize As Integer = 4096) As PageParams

        'struct {
        '    unsigned int object_type; //0xFD1C или 0x01FD1C 
        '    unsigned Int version1; 
        '    unsigned Int version2; 
        '    unsigned Int version3; 
        '    unsigned Long int length; //64-разрядное целое! 
        '    unsigned Int pages[]; 
        '}

        Dim Page = New PageParams With {.PageSize = PageSize}
        Page.PageType = BitConverter.ToInt32(Bytes, 0)
        Page.version = BitConverter.ToInt32(Bytes, 4)
        Page.version1 = BitConverter.ToInt32(Bytes, 8)
        Page.version2 = BitConverter.ToInt32(Bytes, 12)

        If Page.PageType = 64796 Then
            '0xFD1C small storage table
            '???
        ElseIf Page.PageType = 130332 Then
            '0x01FD1C  large storage table
            '???
        End If

        Page.Length = BitConverter.ToInt64(Bytes, 16)

        Dim Index = 24
        Page.PagesNum = New List(Of Integer)

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

        Return Page

    End Function

End Module

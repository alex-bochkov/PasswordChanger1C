Imports System.IO
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
        Dim PageType As Integer
        Dim Length As Long
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
        Dim PageSize As Integer
        Dim TableDefinition As String
        Dim DatabaseVersion As String
        Dim BinaryData() As Byte
    End Structure

    Function ReadInfoBase(FileName As String, TableNameUsers As String) As PageParams

        Dim fs As New FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim reader As New BinaryReader(fs)

        Dim bytesBlock() As Byte = New Byte(24 - 1) {}
        reader.Read(bytesBlock, 0, 24)

        Dim Str = Encoding.UTF8.GetString(bytesBlock, 0, 8)
        Dim V1 = bytesBlock(8).ToString
        Dim V2 = bytesBlock(9).ToString
        Dim V3 = bytesBlock(10).ToString

        Dim DatabaseVersion = V1 + "." + V2 + "." + V3

        Dim DBSize = BitConverter.ToInt32(bytesBlock, 12)
        Dim PageSize = BitConverter.ToInt32(bytesBlock, 20)
        If PageSize = 0 Then
            PageSize = 4096
        End If

        reader.BaseStream.Seek(PageSize, SeekOrigin.Begin)

        If DatabaseVersion.StartsWith("8.3") Then

            Dim Param = DatabaseAccess838.ReadInfoBase(reader, TableNameUsers, PageSize)
            Param.DatabaseVersion = DatabaseVersion

            reader.Close()
            fs.Close()
            Return Param

        Else

            Dim Param = DatabaseAccess8214.ReadInfoBase(reader, TableNameUsers)
            Param.DatabaseVersion = DatabaseVersion

            reader.Close()
            fs.Close()
            Return Param

        End If

        reader.Close()
        fs.Close()
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

    Sub WritePasswordIntoInfoBaseIB(FileName As String, PageHeader As PageParams, UserID As Byte(), OldData As Byte(), NewData As Byte(), DataPos As Integer, DataSize As Integer)

        If PageHeader.DatabaseVersion.StartsWith("8.3") Then
            DatabaseAccess838.WritePasswordIntoInfoBaseIB(FileName, PageHeader, UserID, OldData, NewData, DataPos, DataSize)
            Return
        End If

        Dim PageSize As Integer = PageHeader.PageSize

        Dim fs As New FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Write)
        Dim reader As New BinaryReader(fs)

        Dim bytesBlock1() As Byte = New Byte(PageSize - 1) {}
        reader.BaseStream.Seek(PageHeader.BlockBlob * PageSize, SeekOrigin.Begin)
        reader.Read(bytesBlock1, 0, PageSize)

        Dim DataPage As PageParams = Nothing
        Dim bytesBlock() As Byte

        DataPage = ReadPage(reader, bytesBlock1)

        Dim TotalBlocks = 0
        For Each ST In DataPage.StorageTables
            TotalBlocks = TotalBlocks + ST.DataBlocks.Count
        Next

        bytesBlock = New Byte(PageSize * TotalBlocks - 1) {}

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



        reader.Close()


        Dim NextBlock = DataPos
        Dim Pos = DataPos * 256
        'Dim ByteBlock() As Byte = New Byte(DataSize - 1) {}
        Dim ii = 0
        While NextBlock > 0

            NextBlock = BitConverter.ToInt32(bytesBlock, Pos)
            Dim BlockSize = BitConverter.ToInt16(bytesBlock, Pos + 4)

            For j = 0 To BlockSize - 1
                bytesBlock(Pos + 6 + j) = NewData(ii)
                ii = ii + 1
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

        ii = 0
        For Each ST In DataPage.StorageTables
            For Each DB In ST.DataBlocks
                Dim TempBlock() As Byte = New Byte(PageSize - 1) {}
                For j = 0 To PageSize - 1
                    TempBlock(j) = bytesBlock(ii)
                    ii = ii + 1
                Next

                writer.Seek(DB * PageSize, SeekOrigin.Begin)
                writer.Write(TempBlock)

            Next
        Next


        writer.Close()

    End Sub


End Module

Imports System.Data.SqlClient
Imports System.Security.Principal

Public Class MainForm

    Dim TableParams As AccessFunctions.PageParams

    Structure SQLUser
        Dim ID As Byte()
        Dim IDStr As String
        Dim IDEncoded As String
        Dim Name As String
        Dim Descr As String
        Dim Data As Byte()
        Dim DataStr As String
        Dim PassHash As String
        Dim PassHash2 As String
        Dim AdmRole As String
        Dim KeySize As Integer
        Dim KeyData As Byte()
    End Structure

    Dim SQLUsers As List(Of SQLUser) = New List(Of SQLUser)

    Public Sub New()


        InitializeComponent()

        FileIB.Text = "C:\Users\1C\1Cv8.1CD"

        ConnectionString.Text = "Data Source=MSSQL1;Server=localhost;Integrated Security=true;Database=zup"

    End Sub

    Private Shared Function ShowWarning() As Boolean

        'TEMP
        'Return True

        Dim Rez = MsgBox("Запрещается использование приложения для несанкционированного доступа к данным!" + vbNewLine +
               "Используя данное приложение Вы подтверждаете, что базы данных, к которым будет предоставлен доступ, принадлежат Вашей организации " + vbNewLine +
               "и Вы являетесь Администратором с неограниченным доступом к информации этих баз данных." + vbNewLine +
               "Несанкционированный доступ к информации преследуются по ст. 1301 Гражданского кодекса РФ, ст. 7.12 Кодекса Российской Федерации " + vbNewLine +
               "об административных правонарушениях, ст. 146 Уголовного кодекса РФ." + vbNewLine +
               "Продолжить?",
                         MsgBoxStyle.YesNo, "Правила использования")

        If Rez = MsgBoxResult.Yes Then
            Return True
        End If

        Return False

    End Function

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        OpenFileDialog.FileName = FileIB.Text
        OpenFileDialog.ShowDialog()
        FileIB.Text = OpenFileDialog.FileName

        GetUsers()

    End Sub

    Private Sub ButtonGetUsers_Click(sender As Object, e As EventArgs) Handles ButtonGetUsers.Click
        GetUsers()
    End Sub

    Sub GetUsers()
        'Try

        ListViewUsers.Items.Clear()

        Try

            TableParams = AccessFunctions.ReadInfoBase(FileIB.Text, "V8USERS")

            LabelDatabaseVersion.Text = "Internal database version: " + TableParams.DatabaseVersion

        Catch ex As Exception

            TableParams = Nothing

            MsgBox("Ошибка при попытке чтения данных из файла информационной базы:" + vbNewLine + ex.Message, MsgBoxStyle.Critical, "Ошибка работы с файлом")

            Exit Sub

        End Try

        If TableParams.Records Is Nothing Then
            Exit Sub
        End If

        For Each Row In TableParams.Records

            If Row("NAME").ToString = "" Then
                Row.Add("UserGuidStr", "")
                Row.Add("UserPassHash", "")
                Row.Add("UserPassHash2", "")
                Continue For
            End If

            Dim AuthStructure = ParserServices.ParsesClass.ParseString(Row("DATA"))

            Dim PassHash = AuthStructure(0)(11)

            Dim G = New Guid(DirectCast(Row("ID"), Byte()))

            Row.Add("UserGuidStr", G.ToString)

            'pretty crapy code here..
            If AuthStructure(0)(7) = "0" Then
                Row.Add("UserPassHash", "")
                Row.Add("UserPassHash2", "")
            Else
                If AuthStructure(0).Count = 17 Or TableParams.DatabaseVersion = "8.3.8" Then
                    Row.Add("UserPassHash", AuthStructure(0)(11))
                    Row.Add("UserPassHash2", AuthStructure(0)(12))
                Else
                    Row.Add("UserPassHash", AuthStructure(0)(12))
                    Row.Add("UserPassHash2", AuthStructure(0)(13))
                End If
            End If


            Dim itemUserList = New ListViewItem(G.ToString)

            itemUserList.SubItems.Add(Row("NAME").ToString)
            itemUserList.SubItems.Add(Row("DESCR").ToString)
            itemUserList.SubItems.Add(PassHash)
            itemUserList.SubItems.Add(IIf(Row("ADMROLE"), "Да", ""))

            ListViewUsers.Items.Add(itemUserList)

        Next

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Select Case cbDBType.SelectedIndex
            Case 0
                GetUsersMSSQL()
            Case 1
                GetUsersPostgreSQL()
        End Select

    End Sub

    Sub GetUsersMSSQL()

        '*****************************************************
        SQLUsers.Clear()
        SQLUserList.Items.Clear()

        Try
            Dim Connection = New SqlConnection(ConnectionString.Text)
            Connection.Open()

            Dim command As New SqlCommand("SELECT [ID], [Name], [Descr], [Data], [AdmRole] FROM [dbo].[v8users] ORDER BY [Name]", Connection)

            Dim reader = command.ExecuteReader()

            While reader.Read

                Try

                    Dim SQLUser = New SQLUser
                    SQLUser.ID = reader.GetSqlBinary(0)
                    SQLUser.Name = reader.GetString(1)
                    SQLUser.Descr = reader.GetString(2)
                    SQLUser.Data = reader.GetSqlBinary(3)
                    SQLUser.AdmRole = IIf(BitConverter.ToBoolean(reader.GetSqlBinary(4), 0), "Да", "")

                    SQLUser.IDStr = New Guid(SQLUser.ID).ToString
                    SQLUser.DataStr = CommonModule.DecodePasswordStructure(SQLUser.Data, SQLUser.KeySize, SQLUser.KeyData)

                    Dim AuthStructure = ParserServices.ParsesClass.ParseString(SQLUser.DataStr)

                    If AuthStructure(0)(7).ToString = "0" Then
                        'нет авторизации 1С
                        SQLUser.PassHash = "нет авторизации 1С"
                    Else
                        If AuthStructure(0).Count = 17 Then
                            SQLUser.PassHash = AuthStructure(0)(11).ToString
                            SQLUser.PassHash2 = AuthStructure(0)(12).ToString
                        Else
                            SQLUser.PassHash = AuthStructure(0)(12).ToString
                            SQLUser.PassHash2 = AuthStructure(0)(13).ToString
                        End If
                    End If

                    SQLUsers.Add(SQLUser)

                Catch ex As Exception

                End Try



            End While

            reader.Close()
        Catch ex As Exception

            MsgBox("Ошибка при попытке чтения пользователей из базы данных:" + vbNewLine + ex.Message, MsgBoxStyle.Critical, "Ошибка работы с базой данных")

            Exit Sub

        End Try

        '*****************************************************

        For Each Row In SQLUsers

            If String.IsNullOrEmpty(Row.Name) Then
                Continue For
            End If

            Dim itemUserList = New ListViewItem(Row.IDStr)

            itemUserList.SubItems.Add(Row.Name)
            itemUserList.SubItems.Add(Row.Descr)
            itemUserList.SubItems.Add(Row.PassHash)
            itemUserList.SubItems.Add(Row.AdmRole)

            SQLUserList.Items.Add(itemUserList)

        Next
        '*****************************************************

    End Sub

    Sub GetUsersPostgreSQL()

        '*****************************************************
        SQLUsers.Clear()
        SQLUserList.Items.Clear()

        Try
            Dim Connection = New Odbc.OdbcConnection(ConnectionString.Text)
            Connection.Open()

            'Dim command As New Odbc.OdbcCommand("Select * FROM public.v8users", Connection)
            Dim command As New Odbc.OdbcCommand("Select *, encode(id, 'hex') FROM public.v8users", Connection)

            Dim reader = command.ExecuteReader()

            While reader.Read

                Try

                    Dim SQLUser = New SQLUser
                    SQLUser.ID = New Byte(reader.GetBytes(0, 0, Nothing, 0, 0) - 1) {}
                    Dim a = reader.GetBytes(0, 0, SQLUser.ID, 0, reader.GetBytes(0, 0, Nothing, 0, 0))
                    SQLUser.Name = reader.GetString(1)
                    SQLUser.Descr = reader.GetString(2)
                    SQLUser.Data = New Byte(reader.GetBytes(7, 0, Nothing, 0, 0)) {}
                    SQLUser.IDEncoded = reader.GetString(11)
                    Dim b = reader.GetBytes(7, 0, SQLUser.Data, 0, reader.GetBytes(7, 0, Nothing, 0, 0))
                    SQLUser.AdmRole = IIf(reader.GetString(9) = "1", "Да", "")

                    SQLUser.IDStr = New Guid(SQLUser.ID).ToString
                    SQLUser.DataStr = CommonModule.DecodePasswordStructure(SQLUser.Data, SQLUser.KeySize, SQLUser.KeyData)

                    Dim AuthStructure = ParserServices.ParsesClass.ParseString(SQLUser.DataStr)

                    If AuthStructure(0)(7).ToString = "0" Then
                        'нет авторизации 1С
                        SQLUser.PassHash = "нет авторизации 1С"
                    Else
                        Try
                            If AuthStructure(0).Count = 17 Then
                                SQLUser.PassHash = AuthStructure(0)(11).ToString
                                SQLUser.PassHash2 = AuthStructure(0)(12).ToString
                            Else
                                SQLUser.PassHash = AuthStructure(0)(12).ToString
                                SQLUser.PassHash2 = AuthStructure(0)(13).ToString
                            End If
                        Catch
                        End Try
                    End If

                    SQLUsers.Add(SQLUser)

                Catch ex As Exception

                End Try



            End While

            reader.Close()
        Catch ex As Exception

            MsgBox("Ошибка при попытке чтения пользователей из базы данных:" + vbNewLine + ex.Message, MsgBoxStyle.Critical, "Ошибка работы с базой данных")

            Exit Sub

        End Try

        '*****************************************************

        For Each Row In SQLUsers

            If String.IsNullOrEmpty(Row.Name) Then
                Continue For
            End If

            Dim itemUserList = New ListViewItem(Row.IDStr)

            itemUserList.SubItems.Add(Row.Name)
            itemUserList.SubItems.Add(Row.Descr)
            itemUserList.SubItems.Add(Row.PassHash)
            itemUserList.SubItems.Add(Row.AdmRole)

            SQLUserList.Items.Add(itemUserList)

        Next
        '*****************************************************

    End Sub

    Private Sub ButtonChangePassSQL_Click(sender As Object, e As EventArgs) Handles ButtonChangePassSQL.Click

        If SQLUserList.SelectedItems.Count = 0 Then
            MsgBox("Не выделены строки с пользователями для установки нового пароля!", MsgBoxStyle.Information, "Не выделены строки с пользователями")
            Exit Sub
        End If

        Dim Rez = MsgBox("Внесение изменений в базу данных может привести к непредсказуемым последствиям, вплоть до полного разрушения базы. " + vbNewLine +
                         "Продолжая операцию Вы осознаете это и понимаете, что восстановление будет возможно только из резервной копии." + vbNewLine +
                         "Установить новый пароль выбранным пользователям?", MsgBoxStyle.YesNo, "ВНИМАНИЕ!")

        If Not Rez = MsgBoxResult.Yes Then
            Exit Sub
        End If

        Select Case cbDBType.SelectedIndex
            Case 0
                SetUsersMSSQL()
            Case 1
                SetUsersPostgreSQL()
        End Select

    End Sub

    Sub SetUsersMSSQL()
        Try

            Dim Str = ""

            Dim Connection = New SqlConnection(ConnectionString.Text)
            Connection.Open()

            Dim command As New SqlCommand("UPDATE [dbo].[v8users] SET [Data] = @data WHERE [ID] = @user", Connection)

            For Each item In SQLUserList.SelectedItems

                For Each SQLUser In SQLUsers
                    If SQLUser.IDStr = item.text _
                        And Not SQLUser.PassHash = """""" Then

                        Dim a = 0

                        Str = Str + vbNewLine + SQLUser.Name

                        Dim NewHash = CommonModule.EncryptStringSHA1(NewPassSQL.Text.Trim)

                        Dim NewData = SQLUser.DataStr.Replace(SQLUser.PassHash, """" + NewHash + """")
                        NewData = NewData.Replace(SQLUser.PassHash2, """" + NewHash + """")

                        Dim NewBytes = CommonModule.EncodePasswordStructure(NewData, SQLUser.KeySize, SQLUser.KeyData)

                        command.Parameters.Clear()
                        command.Parameters.Add(New SqlParameter("@user", SqlDbType.Binary)).Value = SQLUser.ID
                        command.Parameters.Add(New SqlParameter("@data", SqlDbType.Binary)).Value = NewBytes

                        command.ExecuteNonQuery()

                    End If
                Next
            Next

            GetUsersMSSQL()

            MsgBox("Успешно установлен пароль '" + NewPassSQL.Text.Trim + "' для пользователей:" + Str, MsgBoxStyle.Information, "Операция успешно выполнена")

        Catch ex As Exception

            MsgBox("Ошибка при попытке записи новых данных пользователей в базу данных:" + vbNewLine + ex.Message, MsgBoxStyle.Critical, "Ошибка работы с базой данных")

        End Try
    End Sub

    Sub SetUsersPostgreSQL()
        Try

            Dim Str = ""
            Dim a = 0

            Dim Connection = New Odbc.OdbcConnection(ConnectionString.Text)
            Connection.Open()

            'Dim command As New Odbc.OdbcCommand("UPDATE public.v8users SET Data = @data WHERE ID = @id", Connection) 'MSSQL syntax
            'Dim command As New Odbc.OdbcCommand("UPDATE public.v8users SET data = ?, name = ? WHERE id = ?", Connection)
            'Dim command As New Odbc.OdbcCommand("UPDATE public.v8users SET data = ?data WHERE id = ?id", Connection) 'MySQL syntax
            'Dim command As New Odbc.OdbcCommand("UPDATE public.v8users SET data = ? WHERE id = decode(encode(?, 'hex'), 'hex')", Connection) 'PostgreSQL syntax

            'Получилось только так, возможно что-то упустил в предыдущих попытках
            Dim command As New Odbc.OdbcCommand("UPDATE public.v8users SET data=? WHERE id=decode(?, 'hex')", Connection) 'PostgreSQL syntax

            For Each item In SQLUserList.SelectedItems

                For Each SQLUser In SQLUsers
                    If SQLUser.IDStr = item.text _
                        And Not SQLUser.PassHash = """""" Then

                        Str = Str + vbNewLine + SQLUser.Name

                        Dim NewHash = CommonModule.EncryptStringSHA1(NewPassSQL.Text.Trim)

                        Dim NewData = SQLUser.DataStr.Replace(SQLUser.PassHash, """" + NewHash + """")
                        NewData = NewData.Replace(SQLUser.PassHash2, """" + NewHash + """")

                        Dim NewBytes = CommonModule.EncodePasswordStructure(NewData, SQLUser.KeySize, SQLUser.KeyData)

                        command.Parameters.Clear()
                        'command.Parameters.Add("@id", Odbc.OdbcType.VarBinary).Value = SQLUser.ID
                        'Важен порядок добавления параметров!
                        command.Parameters.Add("@data", Odbc.OdbcType.Binary).Value = NewBytes
                        command.Parameters.Add("@id", Odbc.OdbcType.Text).Value = SQLUser.IDEncoded
                        'command.Parameters.Add(New Odbc.OdbcParameter("@id", SQLUser.ID))
                        'command.Parameters.Add(New Odbc.OdbcParameter("@data", NewBytes))
                        'command.Parameters.Add(New Odbc.OdbcParameter("id", SqlDbType.Binary)).Value = SQLUser.ID
                        'command.Parameters.Add(New Odbc.OdbcParameter("data", SqlDbType.Binary)).Value = NewBytes
                        'command.Parameters.Add(New Odbc.OdbcParameter("name", Odbc.OdbcType.Binary)).Value = NewBytes

                        a = command.ExecuteNonQuery()

                    End If
                Next
            Next

            GetUsersPostgreSQL()

            If a > 0 Then
                MsgBox("Успешно установлен пароль '" + NewPassSQL.Text.Trim + "' для пользователей:" + Str, MsgBoxStyle.Information, "Операция успешно выполнена")
            End If

        Catch ex As Exception

            MsgBox("Ошибка при попытке записи новых данных пользователей в базу данных:" + vbNewLine + ex.Message, MsgBoxStyle.Critical, "Ошибка работы с базой данных")

        End Try
    End Sub

    Private Sub ButtonRepo_Click(sender As Object, e As EventArgs) Handles ButtonRepo.Click

        OpenFileDialogRepo.FileName = Repo1C.Text
        OpenFileDialogRepo.ShowDialog()
        Repo1C.Text = OpenFileDialogRepo.FileName

        GetUsersRepoUsers()

    End Sub

    Private Sub ButtonGetRepoUsers_Click(sender As Object, e As EventArgs) Handles ButtonGetRepoUsers.Click

        GetUsersRepoUsers()

    End Sub

    Sub GetUsersRepoUsers()

        RepoUserList.Items.Clear()

        Try

            TableParams = AccessFunctions.ReadInfoBase(Repo1C.Text, "USERS")

            LabelDatabaseVersionRepo.Text = "Internal database version: " + TableParams.DatabaseVersion

        Catch ex As Exception

            TableParams = Nothing

            MsgBox("Ошибка при попытке чтения данных из файла хранилища:" + vbNewLine + ex.Message, MsgBoxStyle.Critical, "Ошибка работы с файлом")

            Exit Sub

        End Try

        If TableParams.Records Is Nothing Then
            Exit Sub
        End If

        For Each Row In TableParams.Records

            If Row("NAME").ToString = "" Then
                Continue For
            End If

            Dim G = New Guid(DirectCast(Row("USERID"), Byte()))

            Dim itemUserList = New ListViewItem(G.ToString)

            Row.Add("UserGuidStr", G.ToString)

            itemUserList.SubItems.Add(Row("NAME").ToString)
            If Row("PASSWORD").ToString = "d41d8cd98f00b204e9800998ecf8427e" Then
                itemUserList.SubItems.Add("<нет>")
            Else
                itemUserList.SubItems.Add("пароль установлен")
            End If

            Dim RIGHTS = BitConverter.ToInt32(Row("RIGHTS"), 0)
            If RIGHTS = 65535 Or RIGHTS = 32773 Then
                itemUserList.SubItems.Add("Да")
            End If

            RepoUserList.Items.Add(itemUserList)

        Next

    End Sub

    Private Sub ButtonSetRepoPassword_Click(sender As Object, e As EventArgs) Handles ButtonSetRepoPassword.Click

        If RepoUserList.SelectedItems.Count = 0 Then
            MsgBox("Не выделены строки с пользователями для сброса пароля!", MsgBoxStyle.Information, "Не выделены строки с пользователями")
        Else

            Dim Rez = MsgBox("Внесение изменений в файл хранилища конфигурации может привести к непредсказуемым последствиям, вплоть до полного разрушения базы. " + vbNewLine +
                            "Продолжая операцию Вы осознаете это и понимаете, что восстановление будет возможно только из резервной копии." + vbNewLine +
                            "Установить пустой пароль выбранным пользователям?", MsgBoxStyle.YesNo, "Уверены?")

            If Not Rez = MsgBoxResult.Yes Then
                Exit Sub
            End If

            Try

                Dim Str = ""
                For Each item In RepoUserList.SelectedItems
                    For Each Row In TableParams.Records
                        If Row("UserGuidStr") = item.text Then

                            Str = Str + vbNewLine + Row("NAME").ToString

                            AccessFunctions.WritePasswordIntoInfoBaseRepo(Repo1C.Text, TableParams, DirectCast(Row("USERID"), Byte()), "d41d8cd98f00b204e9800998ecf8427e", Row("OFFSET_PASSWORD"))

                        End If
                    Next
                Next

                GetUsersRepoUsers()

                MsgBox("Успешно установлены пустые пароли для пользователей:" + Str, MsgBoxStyle.Information, "Операция успешно выполнена")

            Catch ex As Exception

                MsgBox("Ошибка при попытке записи данных в файл хранилища:" + vbNewLine + ex.Message, MsgBoxStyle.Critical, "Ошибка работы с файлом")

            End Try

        End If

    End Sub

    Private Sub ButtonChangePwdFileDB_Click(sender As Object, e As EventArgs) Handles ButtonChangePwdFileDB.Click

        If ListViewUsers.SelectedItems.Count = 0 Then
            MsgBox("Не выделены строки с пользователями для сброса пароля!", MsgBoxStyle.Information, "Не выделены строки с пользователями")
        Else

            Dim Rez = MsgBox("Внесение изменений в файл информационной базы может привести к непредсказуемым последствиям, вплоть до полного разрушения базы! " + vbNewLine +
                            "Продолжая операцию Вы осознаете это и понимаете, что восстановление будет возможно только из резервной копии." + vbNewLine +
                            "Установить новый пароль выбранным пользователям?", MsgBoxStyle.YesNo, "ВНИМАНИЕ!")

            If Not Rez = MsgBoxResult.Yes Then
                Exit Sub
            End If

            Try

                Dim Str = ""

                For Each item In ListViewUsers.SelectedItems

                    For Each Row In TableParams.Records

                        If Row("UserGuidStr") = item.text Then

                            Str = Str + vbNewLine + Row("NAME").ToString

                            Dim NewHash = CommonModule.EncryptStringSHA1(NewPassword.Text.Trim)
                            Dim NewHash2 = CommonModule.EncryptStringSHA1(NewPassword.Text.Trim.ToUpper)

                            Dim OldDataBinary = Row("DATA_BINARY")
                            Dim OldData = Row("DATA").ToString
                            Dim NewData = OldData.Replace(Row("UserPassHash"), """" + NewHash2 + """")
                            NewData = NewData.Replace(Row("UserPassHash2"), """" + NewHash2 + """")



                            Dim NewBytes = CommonModule.EncodePasswordStructure(NewData, Row("DATA_KEYSIZE"), Row("DATA_KEY"))

                            AccessFunctions.WritePasswordIntoInfoBaseIB(FileIB.Text, TableParams, DirectCast(Row("ID"), Byte()), OldDataBinary, NewBytes, Row("DATA_POS"), Row("DATA_SIZE"))

                            End If

                    Next

                Next

                GetUsers()

                MsgBox("Успешно установлен пароль '" + NewPassword.Text.Trim + "' для пользователей:" + Str, MsgBoxStyle.Information, "Операция успешно выполнена")

            Catch ex As Exception
                MsgBox("Ошибка при попытке записи данных в файл информационной базы:" + vbNewLine + ex.Message, MsgBoxStyle.Critical, "Ошибка работы с файлом")
            End Try


        End If


    End Sub

    Private Sub MainForm_HelpButtonClicked(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.HelpButtonClicked

        Dim AboutForm = New AboutBox
        AboutForm.ShowDialog()

    End Sub

    Private Sub LinkLabel2_Click(sender As Object, e As EventArgs) Handles LinkLabel2.Click

        Process.Start("https://github.com/alekseybochkov/")

    End Sub

    Private Sub LinkLabel1_Click(sender As Object, e As EventArgs) Handles LinkLabel1.Click

        Process.Start("https://github.com/alekseybochkov/PasswordChanger1C")

    End Sub

    Private Sub MainForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        If Not ShowWarning() Then

            Application.Exit()

        End If

        cbDBType.SelectedIndex = 0

    End Sub

    Private Sub CbDBType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbDBType.SelectedIndexChanged
        Select Case cbDBType.SelectedIndex
            Case 0
                ConnectionString.Text = "Data Source=MSSQL1;Server=localhost;Integrated Security=true;Database=zup"
            Case 1
                ConnectionString.Text = "Driver={PostgreSQL Unicode};Server=localhost;Database=zup;Uid=postgres;Pwd=password;UseServerSidePrepare=1;ReadOnly=0"
        End Select
    End Sub
End Class

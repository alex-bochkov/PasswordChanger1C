<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.Button6 = New System.Windows.Forms.Button()
        Me.FileIB = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.ButtonGetUsers = New System.Windows.Forms.Button()
        Me.ListViewUsers = New System.Windows.Forms.ListView()
        Me.UserGUID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.UserName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.UserDescr = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.UserPassHash = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.UserAdmRole = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ConnectionString = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Repo1C = New System.Windows.Forms.TextBox()
        Me.ButtonRepo = New System.Windows.Forms.Button()
        Me.OpenFileDialogRepo = New System.Windows.Forms.OpenFileDialog()
        Me.ButtonGetRepoUsers = New System.Windows.Forms.Button()
        Me.ButtonSetRepoPassword = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.LabelDatabaseVersion = New System.Windows.Forms.Label()
        Me.NewPassword = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.ButtonChangePwdFileDB = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.ButtonChangePassSQL = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.NewPassSQL = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SQLUserList = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.LabelDatabaseVersionRepo = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.RepoUserList = New System.Windows.Forms.ListView()
        Me.RepoUserGUID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.RepoUserName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.RepoHasPwd = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.RepoAdmin = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button6
        '
        Me.Button6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button6.Location = New System.Drawing.Point(454, 23)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(95, 23)
        Me.Button6.TabIndex = 11
        Me.Button6.Text = "Выбрать файл"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'FileIB
        '
        Me.FileIB.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FileIB.Location = New System.Drawing.Point(9, 25)
        Me.FileIB.Name = "FileIB"
        Me.FileIB.Size = New System.Drawing.Size(443, 20)
        Me.FileIB.TabIndex = 9
        '
        'OpenFileDialog
        '
        Me.OpenFileDialog.Filter = "1C DB files|*.1cd"
        Me.OpenFileDialog.RestoreDirectory = True
        Me.OpenFileDialog.Title = "Выберите файл информационной базы 1С"
        '
        'ButtonGetUsers
        '
        Me.ButtonGetUsers.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonGetUsers.Location = New System.Drawing.Point(551, 23)
        Me.ButtonGetUsers.Name = "ButtonGetUsers"
        Me.ButtonGetUsers.Size = New System.Drawing.Size(183, 23)
        Me.ButtonGetUsers.TabIndex = 12
        Me.ButtonGetUsers.Text = "Получить список пользователей"
        Me.ButtonGetUsers.UseVisualStyleBackColor = True
        '
        'ListViewUsers
        '
        Me.ListViewUsers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListViewUsers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.UserGUID, Me.UserName, Me.UserDescr, Me.UserPassHash, Me.UserAdmRole})
        Me.ListViewUsers.FullRowSelect = True
        Me.ListViewUsers.Location = New System.Drawing.Point(9, 50)
        Me.ListViewUsers.Name = "ListViewUsers"
        Me.ListViewUsers.Size = New System.Drawing.Size(723, 302)
        Me.ListViewUsers.TabIndex = 14
        Me.ListViewUsers.UseCompatibleStateImageBehavior = False
        Me.ListViewUsers.View = System.Windows.Forms.View.Details
        '
        'UserGUID
        '
        Me.UserGUID.Text = "GUID"
        Me.UserGUID.Width = 158
        '
        'UserName
        '
        Me.UserName.Text = "Имя"
        Me.UserName.Width = 164
        '
        'UserDescr
        '
        Me.UserDescr.Text = "Полное имя"
        Me.UserDescr.Width = 147
        '
        'UserPassHash
        '
        Me.UserPassHash.Text = "Хеш пароля"
        Me.UserPassHash.Width = 164
        '
        'UserAdmRole
        '
        Me.UserAdmRole.Text = "Адм.роль"
        Me.UserAdmRole.Width = 66
        '
        'ConnectionString
        '
        Me.ConnectionString.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ConnectionString.Location = New System.Drawing.Point(9, 25)
        Me.ConnectionString.Name = "ConnectionString"
        Me.ConnectionString.Size = New System.Drawing.Size(552, 20)
        Me.ConnectionString.TabIndex = 9
        Me.ConnectionString.Text = "Data Source=MSSQL1;Server=SERVER;Integrated Security=true;Database=DATABASE"
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Location = New System.Drawing.Point(563, 23)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(172, 23)
        Me.Button2.TabIndex = 12
        Me.Button2.Text = "Получить пользователей"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(265, 16)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Файл хранилища конфигурации 1С"
        '
        'Repo1C
        '
        Me.Repo1C.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Repo1C.Location = New System.Drawing.Point(9, 25)
        Me.Repo1C.Name = "Repo1C"
        Me.Repo1C.Size = New System.Drawing.Size(434, 20)
        Me.Repo1C.TabIndex = 9
        '
        'ButtonRepo
        '
        Me.ButtonRepo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRepo.Location = New System.Drawing.Point(445, 23)
        Me.ButtonRepo.Name = "ButtonRepo"
        Me.ButtonRepo.Size = New System.Drawing.Size(98, 23)
        Me.ButtonRepo.TabIndex = 11
        Me.ButtonRepo.Text = "Выбрать файл"
        Me.ButtonRepo.UseVisualStyleBackColor = True
        '
        'OpenFileDialogRepo
        '
        Me.OpenFileDialogRepo.FileName = "OpenFileDialogRepo"
        Me.OpenFileDialogRepo.Filter = "1C DB files|*.1cd"
        Me.OpenFileDialogRepo.Title = "Выберите файл хранилища 1С"
        '
        'ButtonGetRepoUsers
        '
        Me.ButtonGetRepoUsers.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonGetRepoUsers.Location = New System.Drawing.Point(543, 23)
        Me.ButtonGetRepoUsers.Name = "ButtonGetRepoUsers"
        Me.ButtonGetRepoUsers.Size = New System.Drawing.Size(192, 23)
        Me.ButtonGetRepoUsers.TabIndex = 12
        Me.ButtonGetRepoUsers.Text = "Получить список пользователей"
        Me.ButtonGetRepoUsers.UseVisualStyleBackColor = True
        '
        'ButtonSetRepoPassword
        '
        Me.ButtonSetRepoPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonSetRepoPassword.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonSetRepoPassword.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ButtonSetRepoPassword.Location = New System.Drawing.Point(443, 372)
        Me.ButtonSetRepoPassword.Name = "ButtonSetRepoPassword"
        Me.ButtonSetRepoPassword.Size = New System.Drawing.Size(289, 51)
        Me.ButtonSetRepoPassword.TabIndex = 16
        Me.ButtonSetRepoPassword.Text = "Установить выбранным пользователям пустой пароль в хранилище"
        Me.ButtonSetRepoPassword.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(4, 4)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(746, 455)
        Me.TabControl1.TabIndex = 17
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.LabelDatabaseVersion)
        Me.TabPage1.Controls.Add(Me.NewPassword)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.TextBox3)
        Me.TabPage1.Controls.Add(Me.ButtonChangePwdFileDB)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.ListViewUsers)
        Me.TabPage1.Controls.Add(Me.FileIB)
        Me.TabPage1.Controls.Add(Me.Button6)
        Me.TabPage1.Controls.Add(Me.ButtonGetUsers)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(738, 429)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Файловая ИБ"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'LabelDatabaseVersion
        '
        Me.LabelDatabaseVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelDatabaseVersion.AutoSize = True
        Me.LabelDatabaseVersion.Location = New System.Drawing.Point(11, 355)
        Me.LabelDatabaseVersion.Name = "LabelDatabaseVersion"
        Me.LabelDatabaseVersion.Size = New System.Drawing.Size(132, 13)
        Me.LabelDatabaseVersion.TabIndex = 26
        Me.LabelDatabaseVersion.Text = "Internal database version: "
        '
        'NewPassword
        '
        Me.NewPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NewPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NewPassword.Location = New System.Drawing.Point(128, 402)
        Me.NewPassword.Name = "NewPassword"
        Me.NewPassword.Size = New System.Drawing.Size(144, 22)
        Me.NewPassword.TabIndex = 24
        Me.NewPassword.Text = "12345"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(9, 404)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(113, 16)
        Me.Label2.TabIndex = 25
        Me.Label2.Text = "Новый пароль"
        '
        'TextBox3
        '
        Me.TextBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(9, 372)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(431, 29)
        Me.TextBox3.TabIndex = 23
        Me.TextBox3.Text = "Файл информационной базы не должен быть открыт никакими другими приложениями."
        '
        'ButtonChangePwdFileDB
        '
        Me.ButtonChangePwdFileDB.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonChangePwdFileDB.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonChangePwdFileDB.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ButtonChangePwdFileDB.Location = New System.Drawing.Point(443, 373)
        Me.ButtonChangePwdFileDB.Name = "ButtonChangePwdFileDB"
        Me.ButtonChangePwdFileDB.Size = New System.Drawing.Size(289, 51)
        Me.ButtonChangePwdFileDB.TabIndex = 22
        Me.ButtonChangePwdFileDB.Text = "Установить выбранным пользователям указанный пароль"
        Me.ButtonChangePwdFileDB.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(244, 16)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Файл информационной базы 1С"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.TextBox2)
        Me.TabPage2.Controls.Add(Me.ButtonChangePassSQL)
        Me.TabPage2.Controls.Add(Me.Label6)
        Me.TabPage2.Controls.Add(Me.NewPassSQL)
        Me.TabPage2.Controls.Add(Me.Label5)
        Me.TabPage2.Controls.Add(Me.ConnectionString)
        Me.TabPage2.Controls.Add(Me.Button2)
        Me.TabPage2.Controls.Add(Me.SQLUserList)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(738, 429)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Клиент-серверная ИБ (MSSQL)"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(9, 372)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(428, 24)
        Me.TextBox2.TabIndex = 22
        Me.TextBox2.Text = "Монопольного режима доступа к базе не требуется"
        '
        'ButtonChangePassSQL
        '
        Me.ButtonChangePassSQL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonChangePassSQL.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonChangePassSQL.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ButtonChangePassSQL.Location = New System.Drawing.Point(443, 372)
        Me.ButtonChangePassSQL.Name = "ButtonChangePassSQL"
        Me.ButtonChangePassSQL.Size = New System.Drawing.Size(289, 51)
        Me.ButtonChangePassSQL.TabIndex = 21
        Me.ButtonChangePassSQL.Text = "Установить выбранным пользователям указанный пароль"
        Me.ButtonChangePassSQL.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(10, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(293, 16)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "Строка соединения с базой данных 1С"
        '
        'NewPassSQL
        '
        Me.NewPassSQL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NewPassSQL.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NewPassSQL.Location = New System.Drawing.Point(128, 401)
        Me.NewPassSQL.Name = "NewPassSQL"
        Me.NewPassSQL.Size = New System.Drawing.Size(144, 22)
        Me.NewPassSQL.TabIndex = 17
        Me.NewPassSQL.Text = "12345"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(9, 403)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(113, 16)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "Новый пароль"
        '
        'SQLUserList
        '
        Me.SQLUserList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SQLUserList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.SQLUserList.FullRowSelect = True
        Me.SQLUserList.Location = New System.Drawing.Point(9, 50)
        Me.SQLUserList.Name = "SQLUserList"
        Me.SQLUserList.Size = New System.Drawing.Size(723, 319)
        Me.SQLUserList.TabIndex = 20
        Me.SQLUserList.UseCompatibleStateImageBehavior = False
        Me.SQLUserList.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "GUID"
        Me.ColumnHeader1.Width = 158
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Имя"
        Me.ColumnHeader2.Width = 164
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Полное имя"
        Me.ColumnHeader3.Width = 147
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Хеш пароля"
        Me.ColumnHeader4.Width = 164
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Адм.роль"
        Me.ColumnHeader5.Width = 66
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.LabelDatabaseVersionRepo)
        Me.TabPage3.Controls.Add(Me.TextBox1)
        Me.TabPage3.Controls.Add(Me.RepoUserList)
        Me.TabPage3.Controls.Add(Me.Label4)
        Me.TabPage3.Controls.Add(Me.ButtonSetRepoPassword)
        Me.TabPage3.Controls.Add(Me.ButtonGetRepoUsers)
        Me.TabPage3.Controls.Add(Me.Repo1C)
        Me.TabPage3.Controls.Add(Me.ButtonRepo)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(738, 429)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Хранилище конфигурации"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'LabelDatabaseVersionRepo
        '
        Me.LabelDatabaseVersionRepo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelDatabaseVersionRepo.AutoSize = True
        Me.LabelDatabaseVersionRepo.Location = New System.Drawing.Point(12, 357)
        Me.LabelDatabaseVersionRepo.Name = "LabelDatabaseVersionRepo"
        Me.LabelDatabaseVersionRepo.Size = New System.Drawing.Size(132, 13)
        Me.LabelDatabaseVersionRepo.TabIndex = 27
        Me.LabelDatabaseVersionRepo.Text = "Internal database version: "
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(6, 372)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(431, 50)
        Me.TextBox1.TabIndex = 18
        Me.TextBox1.Text = "Файл хранилища конфигурации не должен быть открыт никакими другими приложениями."
        '
        'RepoUserList
        '
        Me.RepoUserList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RepoUserList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.RepoUserGUID, Me.RepoUserName, Me.RepoHasPwd, Me.RepoAdmin})
        Me.RepoUserList.FullRowSelect = True
        Me.RepoUserList.Location = New System.Drawing.Point(9, 50)
        Me.RepoUserList.Name = "RepoUserList"
        Me.RepoUserList.Size = New System.Drawing.Size(723, 304)
        Me.RepoUserList.TabIndex = 17
        Me.RepoUserList.UseCompatibleStateImageBehavior = False
        Me.RepoUserList.View = System.Windows.Forms.View.Details
        '
        'RepoUserGUID
        '
        Me.RepoUserGUID.Text = "GUID"
        Me.RepoUserGUID.Width = 221
        '
        'RepoUserName
        '
        Me.RepoUserName.Text = "Имя"
        Me.RepoUserName.Width = 183
        '
        'RepoHasPwd
        '
        Me.RepoHasPwd.Text = "Пароль установлен"
        Me.RepoHasPwd.Width = 134
        '
        'RepoAdmin
        '
        Me.RepoAdmin.Text = "Это администратор"
        Me.RepoAdmin.Width = 115
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(230, 460)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(282, 13)
        Me.LinkLabel1.TabIndex = 18
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "https://github.com/alekseybochkov/PasswordChanger1C"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 460)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(227, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Страница приложения для обратной связи:"
        '
        'LinkLabel2
        '
        Me.LinkLabel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.Location = New System.Drawing.Point(649, 460)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(102, 13)
        Me.LinkLabel2.TabIndex = 18
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "© Aleksey.Bochkov"
        '
        'MainForm
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.Application
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(752, 475)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.LinkLabel2)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.TabControl1)
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Изменение паролей для информационных баз и хранилища 1С"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button6 As Button
    Friend WithEvents FileIB As TextBox
    Friend WithEvents OpenFileDialog As OpenFileDialog
    Friend WithEvents ButtonGetUsers As Button
    Friend WithEvents ListViewUsers As ListView
    Friend WithEvents UserGUID As ColumnHeader
    Friend WithEvents UserName As ColumnHeader
    Friend WithEvents UserDescr As ColumnHeader
    Friend WithEvents UserPassHash As ColumnHeader
    Friend WithEvents ConnectionString As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents UserAdmRole As ColumnHeader
    Friend WithEvents Label4 As Label
    Friend WithEvents Repo1C As TextBox
    Friend WithEvents ButtonRepo As Button
    Friend WithEvents OpenFileDialogRepo As OpenFileDialog
    Friend WithEvents ButtonGetRepoUsers As Button
    Friend WithEvents ButtonSetRepoPassword As Button
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents RepoUserList As ListView
    Friend WithEvents RepoUserGUID As ColumnHeader
    Friend WithEvents RepoUserName As ColumnHeader
    Friend WithEvents RepoHasPwd As ColumnHeader
    Friend WithEvents RepoAdmin As ColumnHeader
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents NewPassSQL As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents ButtonChangePassSQL As Button
    Friend WithEvents SQLUserList As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ColumnHeader5 As ColumnHeader
    Friend WithEvents ButtonChangePwdFileDB As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents NewPassword As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents Label3 As Label
    Friend WithEvents LinkLabel2 As LinkLabel
    Friend WithEvents LabelDatabaseVersion As Label
    Friend WithEvents LabelDatabaseVersionRepo As Label
End Class

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace DiplomAE
{
    public partial class FormAddAndFilter : Form
    {
        SqlConnection sqlConnection = new SqlConnection(Global.connectionstring);
        public DateTime date;

        // Глобальные переменные для хранения старых значений
        private string oldEmployeeId;
        private string oldUserId;
        private string oldServiceId;
        private string oldDate;
        private string oldStatus;

        private DatabaseHelper databaseHelper;



        public FormAddAndFilter()
        {
            InitializeComponent();


            databaseHelper = new DatabaseHelper(Global.connectionstring);


            employees2.Tag = EmployeeTB;
            recordings2.Tag = ServiceTB2;
            clients.Tag = UserTB;
            clientss.Tag = UserTB2;

            // подсказка для кнопки изменить
            // Создаем объект ToolTip
            System.Windows.Forms.ToolTip toolTip1 = new System.Windows.Forms.ToolTip();
            // Устанавливаем текст подсказки для кнопки
            toolTip1.SetToolTip(ChangeB, "Чтобы изменить существующую запись, выберите строку в основной таблице, и укажите новое значение.");
            toolTip1.SetToolTip(ChangeB2, "Чтобы изменить существующую запись, выберите строку в основной таблице, и укажите новое значение.");
            // обработчики текста поиска


            // обработчики текста поиска
            this.SearchS.Enter += new EventHandler(textbox_Enter);
            this.SearchC.Enter += new EventHandler(textbox_Enter);
            this.SearchS.Leave += new EventHandler(textbox_Leave);
            this.SearchC.Leave += new EventHandler(textbox_Leave);

            this.SearchS2.Enter += new EventHandler(textbox_Enter);
            this.SearchC2.Enter += new EventHandler(textbox_Enter);
            this.SearchS2.Leave += new EventHandler(textbox_Leave);
            this.SearchC2.Leave += new EventHandler(textbox_Leave);



            // обработчики текста для услуг
            this.ProcedureTB.Enter += new EventHandler(textbox_Enter);
            this.DescriptionTB.Enter += new EventHandler(textbox_Enter);
            this.CostTB.Enter += new EventHandler(textbox_Enter);
            this.PhotoTB.Enter += new EventHandler(textbox_Enter);






        }
        // -------------------------------------- с какой формы было открыто
        public string FromForm { get; set; }
        public DataGridView DataGrid { get; set; }

        private void FormAddAndFilter_Load(object sender, EventArgs e)
        {
            
            


            tabControl2.Appearance = TabAppearance.FlatButtons;
            tabControl2.SizeMode = TabSizeMode.Fixed;
            tabControl2.ItemSize = new Size(0, 1);
            if (FromForm == "Admin")
            {
                tabControl2.SelectTab("AdminTab"); // Имя вкладки для админа
                // список услуг  
                string query = "SELECT ProcedureName FROM ListOfServices";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string procedureName = reader["ProcedureName"].ToString();
                    ServiceCB.Items.Add(procedureName);
                }
                reader.Close();
                sqlConnection.Close();
                // заполнение таблиц
                CommonHelper.LoadData(employees2, "SELECT FIO FROM Employee");
                CommonHelper.LoadData(clients, "SELECT FIO FROM Clients");

            }
            else if (FromForm == "Admin2")
            {
                tabControl2.SelectTab("AdminTab2"); // Имя вкладки для админа
            }
            else if (FromForm == "Employee")
            {
                tabControl2.SelectTab("EmployeeTab"); // Имя вкладки для сотрудника
                // список услуг  
                string query = "SELECT Status FROM Appointments";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string procedureName = reader["Status"].ToString();
                    ServiceCB.Items.Add(procedureName);
                }
                reader.Close();
                sqlConnection.Close();
                // заполнение таблиц
                CommonHelper.LoadData(recordings2, "SELECT ProcedureName, Description FROM ListOfServices");
                CommonHelper.LoadData(clientss, "SELECT FIO FROM Clients");
            }
        }

        // -----------------------------------------------------------------------------  поиск
        private void textbox_Enter(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;
            textBox.Text = "";

            if (textBox.ForeColor == Color.Gray)
            {
                textBox.ForeColor = Color.Black;
            }
        }

        private void textbox_Leave(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                switch (textBox.Name)
                {
                    case "SearchS":
                        if (string.IsNullOrWhiteSpace(SearchS.Text))
                        {
                            textBox.Text = "Поиск по сотрудникам:";
                        }
                        break;
                    case "SearchS2":
                        if (string.IsNullOrWhiteSpace(SearchS2.Text))
                        {
                            textBox.Text = "Поиск по услугам:";
                        }
                        break;
                    case "SearchC":
                    case "SearchC2":
                        if (string.IsNullOrWhiteSpace(SearchC.Text) || string.IsNullOrWhiteSpace(SearchC2.Text))
                        {
                            textBox.Text = "Поиск по клиентам:";
                        }
                        break;
                }
                textBox.ForeColor = Color.Gray;
            }
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;

            if (textBox.Focused) // Проверяем, только если TextBox в фокусе
            {
                if (sender == SearchS)
                {
                    CommonHelper.LoadData(employees2, $"SELECT FIO FROM Employee WHERE FIO LIKE '%{SearchS.Text.Replace("'", "''")}%'");
                }
                else if (sender == SearchS2)
                {
                    CommonHelper.LoadData(recordings2, $"SELECT ProcedureName, Description FROM ListOfServices WHERE ProcedureName LIKE '%{SearchS2.Text.Replace("'", "''")}%'");
                }
                else if (sender == SearchC || sender == SearchC2)
                {
                    CommonHelper.LoadData(clients, $"SELECT FIO FROM Clients WHERE FIO LIKE '%{SearchC.Text.Replace("'", "''")}%'");
                    CommonHelper.LoadData(clientss, $"SELECT FIO FROM Clients WHERE FIO LIKE '%{SearchC2.Text.Replace("'", "''")}%'");
                }
            }
        }


        // из списка в текстбокс
        private void CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Проверяем, что выбранная ячейка не заголовок столбца
            {
                DataGridView gridView = sender as DataGridView;
                if (gridView != null)
                {
                    System.Windows.Forms.TextBox targetTextBox = gridView.Tag as System.Windows.Forms.TextBox;
                    if (targetTextBox != null)
                    {
                        DataGridViewRow row = gridView.Rows[e.RowIndex];
                        string cellValue;
                         if (sender == recordings2)
                        {
                            cellValue = row.Cells["ProcedureName"].Value.ToString(); // Получаем значение выбранной ячейки
                        }
                        else
                        {
                            cellValue = row.Cells[e.ColumnIndex].Value.ToString(); // Получаем значение выбранной ячейки
                        }
                        
                        targetTextBox.Text = cellValue; // Записываем значение в TextBox
                    }
                }
            }
        }
        // из календаря в текстбокс
        private void DateChanged(object sender, DateRangeEventArgs e)
        {
            if (sender == monthCalendar1)
            {
                if (monthCalendar1.TodayDate != DateTime.MinValue) // Проверяем, была ли выбрана дата
                {
                    DateTB.Text = monthCalendar1.SelectionRange.Start.ToShortDateString(); // Записываем дату в TextBox

                }
            }
            else if (sender == monthCalendar2)
            {
                if (monthCalendar2.TodayDate != DateTime.MinValue) // Проверяем, была ли выбрана дата
                {
                    DateTB2.Text = monthCalendar2.SelectionRange.Start.ToShortDateString(); // Записываем дату в TextBox

                }
            }
        }


        // ---------------------------------------------------------------------------------

        private void AddA_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.AppStarting;
            tabControl2.Appearance = TabAppearance.FlatButtons;
            tabControl2.SizeMode = TabSizeMode.Fixed;
            tabControl2.ItemSize = new Size(0, 1);
            if (FromForm == "Admin")
            {
                
                try
                {
                    string employeeId = EmployeeTB.Text;
                    string date = DateTB.Text;
                    string time = EventTime.Text;

                    // Проверка на занятое время
                    if (databaseHelper.IsTimeSlotOccupied(employeeId, date, time))
                    {
                        return;
                    }

                    // Код для добавления записи, если время не занято
                    string userId = UserTB.Text;
                    string serviceId = ServiceCB.Text;
                    string status = StatusCB.Text;

                    var validation = Validator.ValidateInputs(
                        ("Сотрудник", employeeId),
                        ("Пользователь", userId),
                        ("Услуга", serviceId),
                        ("Дата", date),
                        ("Время", time),
                        ("Статус", status)
                    );

                    if (!validation.IsValid)
                    {
                        MessageBox.Show(validation.ErrorMessage);
                        return;
                    }

                    string[] columnNames = { "EmployeeId", "UserId", "ServiceId", "Date", "EventTime", "Status" };
                    string[] columnValues = { employeeId, userId, serviceId, date, time, status };

                    string query = "SELECT * FROM Appointments";
                    databaseHelper.InsertOrUpdateRecord(DataGrid, query, "Appointments", columnNames, columnValues, columnValues);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }

            }
            else if (FromForm == "Admin2")
            {
                Cursor = Cursors.AppStarting;
                tabControl2.Appearance = TabAppearance.FlatButtons;
                tabControl2.SizeMode = TabSizeMode.Fixed;
                tabControl2.ItemSize = new Size(0, 1);
                if (FromForm == "Admin2")
                {
                    try
                    {
                        string procedure = ProcedureTB.Text;
                        string description = DescriptionTB.Text;
                        string cost = CostTB.Text;
                        string photo = PhotoTB.Text;
                        string category = CategoryCB.Text;

                        var validation = Validator.ValidateInputs(
                       ("Процедура", procedure),
                       ("Описание", description),
                       ("Стоимость", cost),
                       ("Фото", photo),
                       ("Категория", category)
                        );

                        if (!validation.IsValid)
                        {
                            MessageBox.Show(validation.ErrorMessage);
                            Cursor = Cursors.Default;
                            return;
                        }
                        if (!int.TryParse(cost, out int wcost))
                        {
                            MessageBox.Show("Введите число в строке Стоимость!");
                            return;
                        }

                        int categoryy = 0;
                        switch (category)
                        {
                            case "Женский зал":
                                categoryy = 1;
                                break;
                            case "Мужской зал":
                                categoryy = 2;
                                break;
                            case "Маникюр и педикюр":
                                categoryy = 3;
                                break;

                        }
                        if (categoryy == 0)
                        {
                            MessageBox.Show("Выберите категорию услуги!");
                            Cursor = Cursors.Default;
                            return;
                        }
                        
                        string[] columnNames = { "ProcedureName", "Description", "Cost", "Photo", "CategoryID" };
                        string[] columnValues = { procedure, description, cost, photo, categoryy.ToString() };

                        string query = "SELECT * FROM ListOfServices";
                        databaseHelper.InsertOrUpdateRecord(DataGrid, query, "ListOfServices", columnNames, columnValues, columnValues);
                        

                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show("Произошла ошибка: " + exx.Message);
                    }

                }
                Cursor = Cursors.Default;
            }
            else if (FromForm == "Employee")
            {
                Cursor = Cursors.AppStarting;
                try
                {

                    string EmployeeId = Global.FIO;
                    string date = DateTB2.Text;
                    string time = EventTime2.Text;
                    

                    // Проверка на занятое время
                    if (databaseHelper.IsTimeSlotOccupied(EmployeeId, date, time))
                    {
                        return;
                    }
                    string userId = UserTB2.Text;
                    string serviceId = ServiceTB2.Text;
                    string status = StatusCB2.Text;

                    var validation = Validator.ValidateInputs(
                   ("Сотрудник", EmployeeId),
                   ("Пользователь", userId),
                   ("Услуга", serviceId),
                   ("Дата", date),
                   ("Время", time),
                   ("Статус", status)
                    );

                    if (!validation.IsValid)
                    {
                        MessageBox.Show(validation.ErrorMessage);
                        return;
                    }


                    string[] columnNames = { "EmployeeId", "UserId", "ServiceId", "Date", "EventTime", "Status" };
                    string[] columnValues = { EmployeeId, userId, serviceId, date, time, status };

                    string query = "SELECT * FROM Appointments WHERE EmployeeID LIKE '" + Global.FIO + "'";
                    databaseHelper.InsertOrUpdateRecord(DataGrid, query,"Appointments", columnNames, columnValues, columnValues);
                    
                }
                catch (Exception exx)
                {
                    MessageBox.Show("Произошла ошибка: " + exx.Message);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }


        }


        //-------------------------------------------------------------------------------------------------------------------------изменение
        private void ChangeB_Click(object sender, EventArgs e)
        {
            tabControl2.Appearance = TabAppearance.FlatButtons;
            tabControl2.SizeMode = TabSizeMode.Fixed;
            tabControl2.ItemSize = new Size(0, 1);
            if (FromForm == "Admin")
            {
                try
                {
                    string oldEmployeeId = Global.EmployeeID;
                    string oldUserId = Global.ClientID;
                    string oldServiceId = Global.ServiseID;
                    string oldDate = Global.date.ToString("yyyy-MM-dd HH:mm:ss");
                    string oldTime = Global.time1.ToString();
                    string oldStatus = Global.StatusS;

                    string newEmployeeId = EmployeeTB.Text;
                    string newUserId = UserTB.Text;
                    string newServiceId = ServiceCB.Text;
                    string newDate = DateTB.Text;
                    string newtime = EventTime.Text;
                    string newStatus = StatusCB.Text;


                    // Проверка на занятое время
                    if (databaseHelper.IsTimeSlotOccupied(newEmployeeId, newDate, newtime))
                    {
                        return;
                    }

                    var validation = Validator.ValidateInputs(
                    ("Сотрудник", newEmployeeId),
                    ("Пользователь", newUserId),
                    ("Услуга", newServiceId),
                    ("Дата", newDate),
                     ("Время", newtime),
                    ("Статус", newStatus)
                    );

                    if (!validation.IsValid)
                    {
                        MessageBox.Show(validation.ErrorMessage);
                        return;
                    }

                    if (oldEmployeeId != newEmployeeId || oldUserId != newUserId || oldServiceId != newServiceId || oldDate != newDate || oldStatus != newStatus)
                    {

                        string[] columnNames = { "ID","EmployeeId", "UserId", "ServiceId", "Date", "EventTime", "Status" };
                        string[] newcolumnValues = { Global.procedureid.ToString(),newEmployeeId, newUserId, newServiceId, newDate,newtime, newStatus };
                        string[] oldcolumnValues = { Global.procedureid.ToString(),oldEmployeeId, oldUserId, oldServiceId, oldDate,oldTime, oldStatus };

                        string query = "SELECT * FROM Appointments";
                        databaseHelper.InsertOrUpdateRecord(DataGrid, query, "Appointments", columnNames, newcolumnValues, oldcolumnValues);
                        
                    }
                    else
                    {
                        MessageBox.Show("Нет изменений для обновления.");
                    }
                }
                catch (Exception exx)
                {
                    MessageBox.Show("Произошла ошибка: " + exx.Message);
                }

            }
            else if (FromForm == "Admin2")
            {
                try
                {
                    
                    string newprocedure = ProcedureTB.Text;
                    string newdescription = DescriptionTB.Text;
                    string newcost = CostTB.Text;
                    string newphoto = PhotoTB.Text;
                    string newcategory = CategoryCB.Text;

                    var validation = Validator.ValidateInputs(
                  ("Название услуги", newprocedure),
                  ("Описание услуги", newdescription),
                  ("Стоимость", newcost),
                  ("Имя фотографии", newphoto),
                  ("Номер категории", newcategory)
                   );

                    if (!validation.IsValid)
                    {
                        MessageBox.Show(validation.ErrorMessage);
                        Cursor = Cursors.Default;
                        return;
                    }

                    if (!int.TryParse(newcost, out int cost))
                    {
                        MessageBox.Show("Введите число в строке Стоимость!");
                        return;
                    }
                   

                    
                    /////
                    int categoryy;
                    switch (newcategory)
                    {
                        case "Женский зал":
                            categoryy = 1;
                            break;
                        case "Мужской зал":
                            categoryy = 2;
                            break;
                        case "Маникюр и педикюр":
                            categoryy = 3;
                            break;
                        default:
                            MessageBox.Show("Неверная категория услуги!");
                            return;
                    }
                    MessageBox.Show($"{newprocedure},{newdescription}, {newcost}, {newphoto}, {categoryy}");
                    MessageBox.Show($"{Global.oldprocedure},{Global.olddescription}, {Global.oldcost}, {Global.oldphoto}, {Global.oldcategory}");
                    // Log new values
                   // Console.WriteLine($"newprocedure: {newprocedure}, newdescription: {newdescription}, newcost: {newcost}, newphoto: {newphoto}, categoryy: {categoryy}");

                    if (Global.oldprocedure != newprocedure || Global.olddescription != newdescription || Global.oldcost != newcost || Global.oldphoto != newphoto || Global.oldcategory != newcategory)
                    {
                        string[] columnNames = { "ID", "ProcedureName", "Description", "Cost", "Photo", "CategoryID" };
                        string[] newcolumnValues = { Global.procedureid.ToString(), newprocedure, newdescription, newcost, newphoto, categoryy.ToString() };
                        string[] oldcolumnValues = { Global.procedureid.ToString(), Global.oldprocedure, Global.olddescription, Global.oldcost, Global.oldphoto, Global.oldcategory };

                        // Log old values
                        Console.WriteLine($"oldprocedure: {Global.oldprocedure}, olddescription: {Global.olddescription}, oldcost: {Global.oldcost}, oldphoto: {Global.oldphoto}, oldcategory: {Global.oldcategory}");

                        string query = "SELECT * FROM ListOfServices";
                        databaseHelper.InsertOrUpdateRecord(DataGrid, query, "ListOfServices", columnNames, newcolumnValues, oldcolumnValues);
                        
                    }
                    else
                    {
                        MessageBox.Show("Нет изменений для обновления.");
                    }
                }
                catch (Exception exx)
                {
                    MessageBox.Show("Произошла ошибка: " + exx.Message);
                }
            }
            else if (FromForm == "Employee")
            {
                try
                {
                    string oldEmployeeId = Global.FIO;
                    string oldUserId = Global.ClientID;
                    string oldServiceId = Global.ServiseID;
                    string oldDate = Global.date.ToString("yyyy-MM-dd HH:mm:ss");
                    string oldTime = Global.time1.ToString();
                    string oldStatus = Global.StatusS;

                    string newEmployeeId = Global.FIO;
                    string newUserId = UserTB2.Text;
                    string newServiceId = ServiceTB2.Text;
                    string newDate = DateTB2.Text;
                    string newtime = EventTime2.Text;
                    string newStatus = StatusCB2.Text;

                    // Проверка на занятое время
                    if (databaseHelper.IsTimeSlotOccupied(newEmployeeId, newDate, newtime))
                    {
                        return;
                    }

                    var validation = Validator.ValidateInputs(
                   ("Сотрудник", newEmployeeId),
                   ("Пользователь", newUserId),
                   ("Услуга", newServiceId),
                   ("Дата", newDate),
                   ("Время", newtime),
                   ("Статус", newStatus)
                    );

                    if (!validation.IsValid)
                    {
                        MessageBox.Show(validation.ErrorMessage);
                        Cursor = Cursors.Default;
                        return;
                    }


                    if (oldUserId != newUserId || oldServiceId != newServiceId || oldDate != newDate || oldStatus != newStatus)
                    {

                        string[] columnNames = {"ID", "EmployeeId", "UserId", "ServiceId", "Date", "EventTime", "Status" };
                        string[] newcolumnValues = {Global.procedureid.ToString(), newEmployeeId, newUserId, newServiceId, newDate, newtime, newStatus };
                        string[] oldcolumnValues = {Global.procedureid.ToString(), oldEmployeeId, oldUserId, oldServiceId, oldDate, oldTime, oldStatus };

                        string query = "SELECT * FROM Appointments WHERE EmployeeID LIKE '" + Global.FIO + "'";
                        databaseHelper.InsertOrUpdateRecord(DataGrid, query, "Appointments", columnNames, newcolumnValues, oldcolumnValues);
                       

                    }
                    else
                    {
                        MessageBox.Show("Нет изменений для обновления.");
                    }
                }
                catch (Exception exx)
                {
                    MessageBox.Show("Произошла ошибка: " + exx.Message);
                }

            }


        }


        /*
        private void AddA_Click(object sender, EventArgs e)
        {
            try
            {
                if (EmployeeTB.Text != "Сотрудник" && UserTB.Text != "Пользователь" && ServiceCB.Text != "Услуга" && DateTB.Text != "Дата" && StatusCB.Text != "Статус")
                {
                    string employeeId = EmployeeTB.Text;
                    string userId = UserTB.Text;
                    string serviceId = ServiceCB.Text;
                    string date = DateTB.Text;
                    string status = StatusCB.Text;

                    // Проверка существования записи в базе данных
                    bool recordExists = CheckRecordExists(employeeId, userId, serviceId, date, status);

                    if (recordExists)
                    {
                        DialogResult result = MessageBox.Show("Эта запись уже существует, обновить её?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            Global.EmployeeID = employeeId;
                            Global.ClientID = userId;
                            Global.ServiseID = serviceId;
                            Global.date = DateTime.Parse(date);
                            Global.StatusS = status;
                            MessageBox.Show("Теперь вы можете указать новые данные и нажать на кнопку 'Изменить запись'");
                        }
                    }
                    else
                    {
                        Global.EmployeeID = employeeId;
                        Global.ClientID = userId;
                        Global.ServiseID = serviceId;
                        Global.date = DateTime.Parse(date);
                        Global.StatusS = status;
                        InsertRecord();
                    }
                }
                else
                {
                    MessageBox.Show("Укажите все значения!");
                }
            }
            catch (Exception exx)
            {
                MessageBox.Show("Произошла ошибка: " + exx.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private bool CheckRecordExists(string employeeId, string userId, string serviceId, string date, string status)
        {
            string query = "SELECT COUNT(*) FROM Appointments WHERE EmployeeId = @EmployeeId AND UserId = @UserId AND ServiceId = @ServiceId AND Date = @Date AND Status = @Status";
            int count = 0;
            try
            {
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@ServiceId", serviceId);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Status", status);

                    sqlConnection.Open();
                    count = (int)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            return count > 0;
        }

        private void InsertRecord()
        {
            try
            {
                if (EmployeeTB.Text != "Сотрудник" && UserTB.Text != "Пользователь" && ServiceCB.Text != "Услуга" && DateTB.Text != "Дата" && StatusCB.Text != "Статус")
                {
                    string query = "INSERT INTO Appointments (EmployeeId, UserId, ServiceId, Date, Status) VALUES (@EmployeeId, @UserId, @ServiceId, @Date, @Status)";
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@EmployeeId", EmployeeTB.Text);
                        command.Parameters.AddWithValue("@UserId", UserTB.Text);
                        command.Parameters.AddWithValue("@ServiceId", ServiceCB.Text);
                        command.Parameters.AddWithValue("@Date", DateTB.Text);
                        command.Parameters.AddWithValue("@Status", StatusCB.Text);

                        sqlConnection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Запись добавлена.");
                    }
                }
                else
                {
                    MessageBox.Show("Не все данные указаны.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void ChangeB_Click(object sender, EventArgs e)
        {
            if (EmployeeTB.Text != "Сотрудник" && UserTB.Text != "Пользователь" && ServiceCB.Text != "Услуга" && DateTB.Text != "Дата" && StatusCB.Text != "Статус")
            {
                // Сохраняем старые значения перед проверкой
                string oldEmployeeId = Global.EmployeeID;
                string oldUserId = Global.ClientID;
                string oldServiceId = Global.ServiseID;
                string oldDate = Global.date.ToString("yyyy-MM-dd HH:mm:ss");
                string oldStatus = Global.StatusS;

                string employeeId = EmployeeTB.Text;
                string userId = UserTB.Text;
                string serviceId = ServiceCB.Text;
                string date = DateTB.Text;
                string status = StatusCB.Text;

                // Проверка существования записи в базе данных
                bool recordExists = CheckRecordExists(oldEmployeeId, oldUserId, oldServiceId, oldDate, oldStatus);

                if (recordExists)
                {
                    // Выполнение запроса на обновление данных
                    UpdateRecord(employeeId, userId, serviceId, date, status, oldEmployeeId, oldUserId, oldServiceId, oldDate, oldStatus);
                }
                else
                {
                    MessageBox.Show("Запись для обновления не найдена.");
                }
            }
            else
            {
                MessageBox.Show("Укажите все значения!");
            }
        }

        private void UpdateRecord(string employeeId, string userId, string serviceId, string date, string status, string oldEmployeeId, string oldUserId, string oldServiceId, string oldDate, string oldStatus)
        {
            string query = "UPDATE Appointments SET EmployeeId = @EmployeeId, UserId = @UserId, ServiceId = @ServiceId, Date = @Date, Status = @Status WHERE EmployeeId = @OldEmployeeId AND UserId = @OldUserId AND ServiceId = @OldServiceId AND Date = @OldDate AND Status = @OldStatus";

            try
            {
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@ServiceId", serviceId);
                    command.Parameters.AddWithValue("@Date", DateTime.Parse(date).ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@Status", status);

                    // Добавляем параметры для старых значений
                    command.Parameters.AddWithValue("@OldEmployeeId", oldEmployeeId);
                    command.Parameters.AddWithValue("@OldUserId", oldUserId);
                    command.Parameters.AddWithValue("@OldServiceId", oldServiceId);
                    command.Parameters.AddWithValue("@OldDate", DateTime.Parse(oldDate).ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@OldStatus", oldStatus);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Запись обновлена.");
                    }
                    else
                    {
                        MessageBox.Show("Запись не была обновлена. Проверьте правильность введенных данных.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении записи: {ex.Message}");
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        */

        // применить фильтр
        private void FilterBA_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.AppStarting;
            tabControl2.Appearance = TabAppearance.FlatButtons;
            tabControl2.SizeMode = TabSizeMode.Fixed;
            tabControl2.ItemSize = new Size(0, 1); 

            if (FromForm == "Admin")
            {
                try
                {
                         string filter = "Appointments WHERE ";

                        if (radioButton2.Checked)
                        {
                            filter += "EmployeeID LIKE '" + EmployeeTB.Text + "'";
                        }
                        else if (radioButton3.Checked)
                        {
                            filter += "UserID LIKE '" + UserTB.Text + "'";
                        }
                        else if (radioButton4.Checked)
                        {
                            filter += "ServiceID LIKE '" + ServiceCB.Text + "'";
                        }
                        else if (radioButton1.Checked)
                        {
                            DateTime date;
                            if (DateTime.TryParse(DateTB.Text, out date))
                            {
                                filter += "Date = '" + DateTB.Text + "'";
                            }
                            else
                            {
                                MessageBox.Show("Некорректная дата.");
                                return;
                            }
                        }
                        else if (radioButton5.Checked)
                        {
                            filter += "Status LIKE '" + StatusCB.Text + "'";
                        }
                        else
                        {
                            MessageBox.Show("Выберите флажок возле необходимого фильтра!");
                            return;
                        }

                   

                    FormAdmin formAdmin = Application.OpenForms.OfType<FormAdmin>().FirstOrDefault();
                    if (formAdmin != null)
                    {
                        formAdmin.FromForm2 = filter;
                        formAdmin.ApplyFilter();
                    }

                   
                    
                        
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
            }
            else if (FromForm == "Employee")
            {
                try
                {

                    string filter = " EmployeeID LIKE '" + Global.FIO + "'";
                    string employeeID = Global.FIO;

                    if (radioButton8.Checked)
                    {
                        filter += " AND ServiceID LIKE '" + ServiceTB2.Text + "'";
                    }
                    else if (radioButton7.Checked)
                    {
                        filter += " AND UserID LIKE '" + UserTB2.Text + "'";
                    }
                    else if (radioButton9.Checked)
                    {
                        DateTime date;
                        if (DateTime.TryParse(DateTB2.Text, out date))
                        {
                            filter += " AND Date = '" + DateTB2.Text + "'";
                        }
                        else
                        {
                            MessageBox.Show("Некорректная дата.");
                            return;
                        }
                    }
                    else if (radioButton6.Checked)
                    {
                        filter += " AND Status LIKE '" + StatusCB2.Text + "'";
                    }
                    else
                    {
                        MessageBox.Show("Выберите флажок возле необходимого фильтра!");
                        return;
                    }

                    

                    FormEmployee formEmployee = Application.OpenForms.OfType<FormEmployee>().FirstOrDefault();
                    if (formEmployee != null)
                    {
                        formEmployee.FromForm2 = filter;
                        formEmployee.ApplyFilter();
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
            }
            else if (FromForm == "Admin2")
            {
                try
                {
                    string filter = "ListOfServices WHERE ";

                    if (radioButton13.Checked)
                    {
                        filter += "ProcedureName LIKE '" + ProcedureTB.Text + "'";
                    }
                    else if (radioButton12.Checked)
                    {
                        filter += "Description LIKE '" + DescriptionTB.Text + "'";
                    }
                    else if (radioButton11.Checked)
                    {
                        filter += "Cost LIKE '" + CostTB.Text + "'";
                    }
                    else if (radioButton10.Checked)
                    {
                        filter += "Photo LIKE '" + PhotoTB.Text + "'";
                    }
                    else if (radioButton14.Checked)
                    {
                        int categoryy;
                        switch (CategoryCB.Text)
                        {
                            case "Женский зал":
                                categoryy = 1;
                                break;
                            case "Мужской зал":
                                categoryy = 2;
                                break;
                            case "Маникюр и педикюр":
                                categoryy = 3;
                                break;
                            default:
                                MessageBox.Show(CategoryCB.Text);
                                return;
                        }

                        filter += "CategoryID LIKE '" + categoryy + "'";

                       
                    }
                    else
                    {
                        MessageBox.Show("Выберите флажок возле необходимого фильтра!");
                        return;
                    }



                    FormAdmin formAdmin = Application.OpenForms.OfType<FormAdmin>().FirstOrDefault();
                    if (formAdmin != null)
                    {
                        formAdmin.FromForm2 = filter;
                        formAdmin.ApplyFilter();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
            }


            Cursor = Cursors.Default;

        }




























    }
}

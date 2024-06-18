using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiplomAE
{
    public partial class FormAdmin
    {
        // ------------------------------------------------------------------------------------- модерация
        // Отзывы
        private void GotoAllBtn_Click(object sender, EventArgs e)
        {
            GotoUncheckedBtn.Visible = true;
            GotoAllBtn.Visible = false;
            CommonHelper.LoadData(ReviewsDGV, "SELECT * FROM Reviews WHERE Checked =1");
        }

        private void GotoUncheckedBtn_Click(object sender, EventArgs e)
        {
            GotoUncheckedBtn.Visible = false;
            GotoAllBtn.Visible = true;
            CommonHelper.LoadData(ReviewsDGV, "SELECT * FROM Reviews WHERE Checked = 0");
        }

        private void ReviewsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = ReviewsDGV.Rows[e.RowIndex];
                HeaderTB.Text = selectedRow.Cells["Header"].Value.ToString();
                TextRTB.Text = selectedRow.Cells["Text1"].Value.ToString();
                string ifckeched = selectedRow.Cells["Checked"].Value.ToString();
                if (ifckeched == "False")
                {
                    CheckedCB.Text = "Не проверен";
                }
                else
                {
                    CheckedCB.Text = "Проверен";
                }
            }



        }

        private void SaveRB_Click(object sender, EventArgs e)
        {
            sqlConnection.Open();


            if (HeaderTB.Text != "Заголовок:" && TextRTB.Text != "Текст:" && CheckedCB.Text != "Статус:")
            {

                string query = "UPDATE Reviews SET Header = @Header, Text = @Text, Checked = @Checked WHERE ID = @id";
                SqlCommand command = new SqlCommand(query, sqlConnection);

                int id = Convert.ToInt32(ReviewsDGV.CurrentRow.Cells["ID3"].Value);

                command.Parameters.AddWithValue("@Header", HeaderTB.Text);
                command.Parameters.AddWithValue("@Text", TextRTB.Text);
                string ifckeched = CheckedCB.Text;
                if (ifckeched == "Проверен")
                {
                    command.Parameters.AddWithValue("@Checked", true);
                }
                else if (ifckeched == "Не проверен")
                {
                    command.Parameters.AddWithValue("@Checked", false);
                }
                else
                {
                    MessageBox.Show("Вы не указали статус!");
                    return;
                }

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

               

                MessageBox.Show("Статус отзыва изменен.");
                if (GotoAllBtn.Visible == true)
                {
                    CommonHelper.LoadData(ReviewsDGV, "SELECT * FROM Reviews WHERE Checked = 0");
                }
                else
                {
                    CommonHelper.LoadData(ReviewsDGV, "SELECT * FROM Reviews WHERE Checked =1");
                }
            }
            else
            {
                MessageBox.Show("Вы не заполнили все данные!");
            }

            sqlConnection.Close();
        }

        private void DenyRB_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить отзыв?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                sqlConnection.Open();
                using (SqlConnection sqlConnection1 = sqlConnection)
                {

                    string query = "DELETE FROM Reviews WHERE ID = @ID";
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    int id = Convert.ToInt32(ReviewsDGV.CurrentRow.Cells["ID3"].Value);
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();

                    if (GotoAllBtn.Visible == true)
                    {
                        CommonHelper.LoadData(ReviewsDGV, "SELECT * FROM Reviews WHERE Checked = 0");
                    }
                    else
                    {
                        CommonHelper.LoadData(ReviewsDGV, "SELECT * FROM Reviews WHERE Checked =1");
                    }
                    MessageBox.Show("Отзыв удален.");
                }

                sqlConnection.Close();

            }
        }
        // FAQ
        private void FAQDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = FAQDGV.Rows[e.RowIndex];
                QuestionRTB.Text = selectedRow.Cells["TextFAQ"].Value.ToString();
                AnswerRTB.Text = selectedRow.Cells["Answer"].Value.ToString();
            }
        }

        private void AnswerB_Click(object sender, EventArgs e)
        {
            sqlConnection.Open();


            if (AnswerRTB.Text != null)
            {

                string query = "UPDATE FAQ SET Answer = @Answer WHERE ID = @id";
                SqlCommand command = new SqlCommand(query, sqlConnection);

                int id = Convert.ToInt32(FAQDGV.CurrentRow.Cells["ID4"].Value);

                command.Parameters.AddWithValue("@Answer", AnswerRTB.Text);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
                MessageBox.Show("Ответ добавлен!");

                CommonHelper.LoadData(FAQDGV, "SELECT * FROM FAQ ");

            }
            else
            {
                MessageBox.Show("Введите сообщение ответа!");
            }

            sqlConnection.Close();

        }

        private void DeleteQB_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить вопрос?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    
                    using (SqlConnection sqlConnection1 = new SqlConnection(Global.connectionstring))
                    {
                        sqlConnection1.Open();
                        string query = "DELETE FROM FAQ WHERE ID = @ID";
                        SqlCommand command = new SqlCommand(query, sqlConnection1);
                        int id = Convert.ToInt32(FAQDGV.CurrentRow.Cells["ID4"].Value);
                        command.Parameters.AddWithValue("@ID", id);
                        command.ExecuteNonQuery();

                        CommonHelper.LoadData(FAQDGV, "SELECT * FROM FAQ ");
                        MessageBox.Show("Вопрос удален.");

                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                

               

            }
        }





    }




}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EmployeeManagementSystem
{
    public partial class Dashboard : UserControl
    {
        private readonly SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Andassa\Documents\employee.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False");

        public Dashboard()
        {
            InitializeComponent();
            SetupCharts();
            RefreshData();
        }

        private void SetupCharts()
        {
            // Configure the pie chart
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("MainArea1");
            chart1.ChartAreas.Add(chartArea1);
            Series series1 = new Series("StatusDistribution")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true
            };
            chart1.Series.Add(series1);
            chart1.Titles.Clear();
            chart1.Titles.Add("Employee Status Distribution");

            // Configure the bar chart
            chart2.Series.Clear();
            chart2.ChartAreas.Clear();
            ChartArea chartArea2 = new ChartArea("MainArea2");
            chart2.ChartAreas.Add(chartArea2);
            Series series2 = new Series("StatusPercentage")
            {
                ChartType = SeriesChartType.Bar,
                IsValueShownAsLabel = true,
                LabelFormat = "{0}%"
            };
            chart2.Series.Add(series2);
            chart2.Titles.Clear();
            chart2.Titles.Add("Employee Status Percentages");
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;
            }

            displayTE();
            displayAE();
            displayIE();
            UpdateCharts();
        }

        public void displayTE()
        {
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();
                    string selectData = "SELECT COUNT(id) FROM employees WHERE delete_date IS NULL";
                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        dashboard_TE.Text = count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        public void displayAE()
        {
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();
                    string selectData = "SELECT COUNT(id) FROM employees WHERE status = @status AND delete_date IS NULL";
                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        cmd.Parameters.AddWithValue("@status", "Active");
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        dashboard_AE.Text = count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        public void displayIE()
        {
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();
                    string selectData = "SELECT COUNT(id) FROM employees WHERE status = @status AND delete_date IS NULL";
                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        cmd.Parameters.AddWithValue("@status", "Inactive");
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        dashboard_IE.Text = count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        private void UpdateCharts()
        {
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    int total = GetCount("SELECT COUNT(id) FROM employees WHERE delete_date IS NULL");
                    int active = GetCount("SELECT COUNT(id) FROM employees WHERE status = 'Active' AND delete_date IS NULL");
                    int inactive = GetCount("SELECT COUNT(id) FROM employees WHERE status = 'Inactive' AND delete_date IS NULL");

                    // Update the pie chart
                    chart1.Series["StatusDistribution"].Points.Clear();
                    chart1.Series["StatusDistribution"].Points.AddXY("Active", active);
                    chart1.Series["StatusDistribution"].Points.AddXY("Inactive", inactive);

                    // Update the bar chart with percentages
                    double activePercentage = total > 0 ? (active / (double)total) * 100 : 0;
                    double inactivePercentage = total > 0 ? (inactive / (double)total) * 100 : 0;

                    chart2.Series["StatusPercentage"].Points.Clear();
                    chart2.Series["StatusPercentage"].Points.AddXY("Active", activePercentage);
                    chart2.Series["StatusPercentage"].Points.AddXY("Inactive", inactivePercentage);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        private int GetCount(string query)
        {
            int count = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving count: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return count;
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            // Handle chart1 click event if needed
        }

        private void chart2_Click(object sender, EventArgs e)
        {
            // Handle chart2 click event if needed
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Ajoutez ici la logique nécessaire pour le dessin de panel1, si nécessaire.
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            // Ajoutez ici la logique nécessaire pour le dessin de panel2, si nécessaire.
        }
    }
}

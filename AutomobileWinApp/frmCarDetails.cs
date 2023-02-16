using AutomobileLibrary.BusinessObject;
using AutomobileLibrary.Repository;
namespace AutomobileWinApp
{
    public partial class frmCarDetails : Form
    {
        public frmCarDetails()
        {
            InitializeComponent();
        }
        //------------------------------------
        public ICarRepository CarRepository { get; set; }
        public bool InsertOrUpdate { get; set; }//False: Insert, True: Update
        public Car CarInfo { get; set; }

        //------------------------------------
        private void frmCarDetails_Load(object sender, EventArgs e)
        {
            cbManufacturer.SelectedIndex = 0;
            txtCarID.Enabled = !InsertOrUpdate;
            if (InsertOrUpdate==true)
            {
                txtCarID.Text = CarInfo.CarID.ToString();
                txtCarName.Text = CarInfo.CarName.ToString();
                cbManufacturer.Text = CarInfo.Manufacturer.ToString();
                txtPrice.Text = CarInfo.Price.ToString();
                txtReleaseYear.Text = CarInfo.ReleaseYear.ToString();
            }

            
        }
        //------------------------------------
        private void btnSave_Click(object sender, EventArgs e)
        {
            try { 
                var car = new Car
                {
                    CarID = int.Parse(txtCarID.Text),
                    CarName = txtCarName.Text,
                    Manufacturer = cbManufacturer.Text,
                    Price = decimal.Parse(txtPrice.Text),
                    ReleaseYear = int.Parse(txtReleaseYear.Text)
                };
                if (InsertOrUpdate == false)
                {
                    CarRepository.InsertCar(car);
                }
                else
                {
                    CarRepository.UpdateCar(car);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, InsertOrUpdate ? "Update a car" : "Add a new car");
            }
        }
        //------------------------------------
        private void btnCancel_Click(object sender, EventArgs e) => Close();
    }//end class
}//end namespace
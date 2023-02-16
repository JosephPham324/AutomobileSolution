using AutomobileLibrary.Repository;
using AutomobileLibrary.BusinessObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomobileWinApp
{
    public partial class frmCarManagement : Form
    {
        ICarRepository carRepository = new CarRepository();
        public frmCarManagement()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        BindingSource source;

        //------------------------------------
        private void frmCarManagement_Load(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;
            //Register this event to open frmCarDetails form that performs updating
            dgvCarList.CellDoubleClick += dgvCarList_CellDoubleClick;
        }
        //------------------------------------
        private void dgvCarList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmCarDetails frmCarDetails = new frmCarDetails
            {
                Text = "Update Car",
                InsertOrUpdate = true,
                CarInfo = GetCarObject(),
                CarRepository = carRepository
            };  

            if (frmCarDetails.ShowDialog() == DialogResult.OK)
            {
                LoadCarList();
                source.Position = source.Count - 1;
            }   
        }
        //------------------------------------
        private void ClearText()
        {
            txtCarID.Text = string.Empty;
            txtCarName.Text = string.Empty;
            txtManufacturer.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtReleaseYear.Text = string.Empty;
        }
        //------------------------------------
        private Car GetCarObject()
        {
            Car car = null;
            try
            {
                car = new Car()
                {
                    CarID = int.Parse(txtCarID.Text),
                    CarName = txtCarName.Text,
                    Manufacturer = txtManufacturer.Text,
                    Price = decimal.Parse(txtPrice.Text),
                    ReleaseYear = int.Parse(txtReleaseYear.Text)
                };
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Car");
            }
            return car;
        }
        //------------------------------------
        public void LoadCarList()
        {
            var cars = carRepository.GetCars();

            try
            {
                source = new BindingSource();
                source.DataSource = cars;

                txtCarID.DataBindings.Clear();
                txtCarName.DataBindings.Clear();
                txtManufacturer.DataBindings.Clear();
                txtPrice.DataBindings.Clear();
                txtReleaseYear.DataBindings.Clear();

                txtCarID.DataBindings.Add("Text", source, "CarID");
                txtCarName.DataBindings.Add("Text", source, "CarName");
                txtManufacturer.DataBindings.Add("Text", source, "Manufacturer");
                txtPrice.DataBindings.Add("Text", source, "Price");
                txtReleaseYear.DataBindings.Add("Text", source, "ReleaseYear");

                dgvCarList.DataSource = null;
                dgvCarList.DataSource = source;
                if (cars.Count() == 0)
                {
                    ClearText();
                    btnDelete.Enabled = false;
                }
                else
                {
                    btnDelete.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Car List");
            }
        }
        //------------------------------------
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadCarList();
        }
        //------------------------------------
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmCarDetails frmCarDetails = new frmCarDetails
            {
                Text = "Add a car",
                InsertOrUpdate = false,
                CarRepository = carRepository
            };
            if (frmCarDetails.ShowDialog() == DialogResult.OK)
            {
                LoadCarList();
                source.Position = source.Count - 1;
            }
        }
        //------------------------------------
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var car = GetCarObject();
                carRepository.DeleteCar(car.CarID);
                LoadCarList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete a car");
            }
        }
        //------------------------------------
        private void btnClose_Click(object sender, EventArgs e) => Close();
    }//end class
}//end namespace

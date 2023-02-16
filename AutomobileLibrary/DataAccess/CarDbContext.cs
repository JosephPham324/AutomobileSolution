using AutomobileLibrary.BusinessObject;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomobileLibrary.DataAccess
{
    public class CarDbContext: BaseDAL
    {
        //---------------------------------
        //Using Singleton pattern 
        private static CarDbContext instance = null;
        private static readonly object instanceLock = new object();
        private CarDbContext() { }
        public static CarDbContext Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CarDbContext();
                    }
                    return instance;
                }
            }
        }
        //---------------------------------
        public IEnumerable<Car> GetCarList()
        {
            IDataReader dataReader = null;
            string SQLSelect = "SELECT CarID, CarName, Manufacturer, Price, ReleasedYear from Cars";
            var cars = new List<Car>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read()) {
                    cars.Add(new Car
                    {
                        CarID = dataReader.GetInt32(0),
                        CarName = dataReader.GetString(1),
                        Manufacturer = dataReader.GetString(2),
                        Price = dataReader.GetDecimal(3),
                        ReleaseYear = dataReader.GetInt32(4)
                    });
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return cars;
        }
        //---------------------------------
        public Car GetCarByID(int carID)
        {
            //using LINQ to Object
            Car car = null;
            IDataReader dataReader = null;
            string SQLSelect = "SELECT CarID, CarName, Manufacturer, Price, ReleasedYear from Cars where CarID = @CarID";
            var cars = new List<Car>();
            try
            {
                var param = dataProvider.CreateParameter("@CarID", 4, carID, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    car = new Car
                    {
                        CarID = dataReader.GetInt32(0),
                        CarName = dataReader.GetString(1),
                        Manufacturer = dataReader.GetString(2),
                        Price = dataReader.GetDecimal(3),
                        ReleaseYear = dataReader.GetInt32(4)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return car;

        }
        //---------------------------------
        //Add a new car
        public void AddCar(Car car)
        {
            Car product = GetCarByID(car.CarID);
            if (product is not null)
            {
                throw new Exception("Car already exists!");
            }
            string SQLInsert = "INSERT INTO Cars VALUES (@CarID, @CarName, @Manufacturer, @Price, @ReleasedYear)";
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@CarID",4,car.CarID, DbType.Int32)); 
                parameters.Add(dataProvider.CreateParameter("@CarName",50,car.CarName, DbType.String)); 
                parameters.Add(dataProvider.CreateParameter("@Manufacturer",50,car.Manufacturer, DbType.String)); 
                parameters.Add(dataProvider.CreateParameter("@Price",50,car.Price, DbType.Decimal)); 
                parameters.Add(dataProvider.CreateParameter("@ReleasedYear",4,car.ReleaseYear, DbType.Int32));
                dataProvider.Insert(SQLInsert, CommandType.Text, parameters.ToArray());
            }
            catch(Exception ex)
            {
                throw new Exception (ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        //------------------------------------
        //Update a car
        public void Update(Car car)
        {
            Car product = GetCarByID(car.CarID);
            if (product is null)
            {
                throw new Exception("Car doesn't exists!");
            }
            string SQLInsert = "UPDATE Cars " +
                "SET CarName = @CarName, Manufacturer = @Manufacturer, Price = @Price, ReleasedYear= @ReleasedYear " +
                "WHERE CarID = @CarID";
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@CarID", 4, car.CarID, DbType.Int32));
                parameters.Add(dataProvider.CreateParameter("@CarName", 50, car.CarName, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@Manufacturer", 50, car.Manufacturer, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@Price", 50, car.Price, DbType.Decimal));
                parameters.Add(dataProvider.CreateParameter("@ReleasedYear", 4, car.ReleaseYear, DbType.Int32));
                dataProvider.Update(SQLInsert, CommandType.Text, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        //---------------------------------
        //Remove a car
        public void Remove(int CarID)
        {
            Car product = GetCarByID(CarID);
            if (product is null)
            {
                throw new Exception("Car doesn't exist!");
            }
            string SQLDelete = "DELETE FROM Cars WHERE CarID = @CarID";
            try
            {
                var parameter = dataProvider.CreateParameter("@CarID",4,CarID, DbType.String);
                dataProvider.Delete(SQLDelete, CommandType.Text, parameter);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
    }//end class
}//end namespace

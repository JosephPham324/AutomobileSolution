using AutomobileLibrary.BusinessObject;
using AutomobileLibrary.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomobileLibrary.Repository
{
    public class CarRepository : ICarRepository
    {
        public IEnumerable<Car> GetCars() => CarDbContext.Instance.GetCarList();
        public Car GetCarByID(int carId) => CarDbContext.Instance.GetCarByID(carId);
        public void InsertCar(Car car) => CarDbContext.Instance.AddCar(car);
        public void UpdateCar(Car car) => CarDbContext.Instance.Update(car);
        public void DeleteCar(int carId) => CarDbContext.Instance.Remove(carId);
    }//end class
}//end namespace

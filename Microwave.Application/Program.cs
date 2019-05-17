using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;

namespace Microwave.Application
{
    public class Program
    {
        static void Main(string[] args)
        {
         

            var powerButton = new Button();
            var timeButton = new Button();
            var startCancelButton = new Button();
            var door = new Door();
            var output = new Output();
            var display = new Display(output);
            var timer = new Timer();
            var powerTube = new PowerTube(output);
            var cookController = new CookController(timer, display, powerTube);
            var light = new Light(output);
            var userInterface = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light,
                cookController);
            cookController.UI = userInterface;

            Console.WriteLine("User controls enabled. Press esc to exit." +
                              "\nPress P to set power. T to set time. S to start the microwave" +
                              "\nPress D to open the door. C to close the door");
            while (true)
            {
                ConsoleKeyInfo keyyy = Console.ReadKey();
                if (keyyy.Key == ConsoleKey.P)
                {
                    powerButton.Press();
                }
                else if (keyyy.Key == ConsoleKey.T)
                {
                    timeButton.Press();
                }
                else if (keyyy.Key == ConsoleKey.S)
                {
                    startCancelButton.Press();
                }
                else if (keyyy.Key == ConsoleKey.D)
                {
                    door.Open();
                }
                else if (keyyy.Key == ConsoleKey.C)
                {
                    door.Close();
                }
                else if (keyyy.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}

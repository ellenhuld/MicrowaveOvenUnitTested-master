using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace MicrowaveOven.Tests.Integration
{
    [TestFixture]
    public class Step2_CookControllerTimer
    {
        private ICookController _cookController;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IDisplay _display;
        private IOutput _output;


        [SetUp] // Definerer stubbe og rigtige klasser
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _display = Substitute.For<IDisplay>();
            _powerTube = new PowerTube(_output);
            _timer = new Timer();

            _cookController = new CookController(_timer, _display, _powerTube);
        }

        [Test]
        public void Timer_Start_RemainingTime()
        {
            _cookController.StartCooking(50,10);
            Assert.That(_timer.TimeRemaining, Is.EqualTo(10));
        }

        [Test]
        public void Timer_TimeExpire_Off()
        {
            _cookController.StartCooking(50,1);
            Thread.Sleep(1500);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }
    }





}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;
using NSubstitute;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace MicrowaveOven.Tests.Integration
{
    [TestFixture()]
    class Step7_UserInterfaceLight
    {

        private CookController _cookController;
        private IUserInterface _userInterface;
        private ILight _light;
        private IDisplay _display;
        private IOutput _output;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private ITimer _timer;
        private IPowerTube _powerTube;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _light = new Light(_output);
            _display = Substitute.For<IDisplay>();
            _timer = new MicrowaveOvenClasses.Boundary.Timer();
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }


        [Test]
        public void DoorOpen_LightOn()
        {
            _door.Open();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("on")));

        }

        [Test]
        public void DoorOpen_DoorClose_LightOff()
        {
            _door.Open();
            _door.Close();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));

        }

        [Test]
        public void StartCooking_LightOn()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void StartCooking_LightNotOff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => !str.Contains("off")));
        }

        [Test]
        public void StartCooking_StopCooking_LightOff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void StartCooking_TimeExpired_LightOff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(61000);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }
    }
}

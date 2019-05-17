using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using System.Threading;


namespace MicrowaveOven.Tests.Integration
{
    [TestFixture()]
    class IT6UserInterfaceButton
    {
        private CookController _cookController;
        private IDisplay _display;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IOutput _output;
        private UserInterface _userInterface;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private ILight _light;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _display = Substitute.For<IDisplay>();
            _timer = new MicrowaveOvenClasses.Boundary.Timer();
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = Substitute.For<IDoor>();
            _light = Substitute.For<ILight>();
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display,
                _light,
                _cookController);
            _cookController.UI = _userInterface;
        }

        [Test]
        public void powerPressOnce_Start_Power50()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("50")));
        }

        [Test]
        public void powerPressTwice_Start_Power100()
        {
            _powerButton.Press();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("100")));
        }

        [Test]
        public void timePressOnce_wait59_Power50()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(59000);
            _output.Received().OutputLine(Arg.Is<string>(str => !str.Contains("off")));
        }

        // Fejlede fordi cookcontroller ikke kaldte TurnOff til PowerTube. Det er rettet nu

        [Test]
        public void timePressOnce_wait61_PowerOff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(61000);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void timePressTwice_wait199_Power50()
        {
            _powerButton.Press();
            _timeButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(119000);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("50")));
        }

        [Test]
        public void timePressTwice_wait121_Poweroff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(millisecondsTimeout: 121000);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));

        }

        [Test]
        public void Start_Stop_PowerOff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }
    }
}
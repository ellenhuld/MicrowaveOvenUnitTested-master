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

namespace MicrowaveOven.Tests.Integration
{
    [TestFixture()]
    class Step4_CookControlerUserInterface
    {
        private CookController _cookController;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IDisplay _display;
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
            _powerTube = new PowerTube(_output);
            _timer = new MicrowaveOvenClasses.Boundary.Timer();
            _cookController = new CookController(_timer, _display, _powerTube);
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            _light = Substitute.For<ILight>();
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }

        [Test]
        public void UserInterface_OnPowerPressed1_Power()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("50")));
        }

        [Test]
        public void UserInterface_OnPowerPressed2_Power()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("100")));
        }
        [Test]
        public void UserInterface_Stop_Power()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void UserInterface_OnTimerExpired_ClearDisplay()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(61000);

            _display.Received().Clear();
        }

    }
}

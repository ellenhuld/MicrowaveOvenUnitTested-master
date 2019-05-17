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




namespace MicrowaveOven.Tests.Integration
{
    [TestFixture()]
    class Step5_UserInterfaceDoor
    {
        
        private ICookController _cookController;
        private IDisplay _display;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IOutput _output;
        private IUserInterface _userInterface;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private ILight _light;

        public ICookController CookController { get => _cookController; set => _cookController = value; }

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _display = Substitute.For<IDisplay>();
            _timer = Substitute.For<ITimer>();
            _powerTube = new PowerTube(_output);
            CookController = new CookController(_timer, _display, _powerTube);
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _door = new Door();
            _light = Substitute.For<ILight>();
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light,
                CookController);
        }

        [Test]
        public void Start_Open_PowerOff()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _door.Open();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void Start_OpenClose_Start_PowerDefault()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _door.Open();
            _door.Close();

            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("50")));
        }

    }
}

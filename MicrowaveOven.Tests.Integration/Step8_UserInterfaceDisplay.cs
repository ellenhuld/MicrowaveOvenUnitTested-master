using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.Core;
using Timer = MicrowaveOvenClasses.Boundary.Timer;
using NUnit.Framework;
using NUnit.Framework.Internal;


namespace MicrowaveOven.Tests.Integration
{
    [TestFixture()]
    class Step8_UserInterfaceDisplay
    {
        private Display _display;
        private UserInterface _ui;
        private Light _light;
        private Button _powerButton;
        private Button _startCancelButton;
        private Button _timerButton;
        private Door _door;
        private IOutput _output;
        private Timer _timer;
        private PowerTube _powerTube;
        private CookController _cookController;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _powerButton = new Button();
            _timerButton = new Button();
            _startCancelButton = new Button();
            _light = new Light(_output);
            _timer = new Timer();
            _door = new Door();
            _cookController = new CookController(_timer, _display, _powerTube);
            _ui = new UserInterface(_powerButton, _timerButton, _startCancelButton, _door, _display, _light,
                _cookController);
            _cookController.UI = _ui;
        }

        [Test]
        public void PowerPress_Power50()
        {
            _powerButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("50")));

        }

        [Test]
        public void TimePress_strTime()
        {
            _powerButton.Press();
            _timerButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("01:00")));

        }

        [Test]
        public void PowerPress_DoorOpen_DisplayClear()
        {
            _powerButton.Press();
            _door.Open();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }



        [Test]
        public void Start_TimeExpire_DisplayClear()
        {
            _powerButton.Press();
            _timerButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(61000);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }
    }
}

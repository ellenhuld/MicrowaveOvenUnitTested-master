using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace MicrowaveOven.Tests.Integration
{
    [TestFixture]
    public class Step3_CookControlerDisplay
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
            _display = new Display(_output);
            _powerTube = Substitute.For<IPowerTube>();
            _timer = Substitute.For<ITimer>();
            _cookController = new CookController(_timer, _display, _powerTube);
        }

        [TestCase(60, "01:00")]
        [TestCase(59, "00:59")]
        [TestCase(61, "01:01")]
        public void Display_StartTime_ShowTimeRemaining(int time, string outputStr)
        {
            _cookController.StartCooking(50, time);
            _timer.TimeRemaining.Returns(time);
            _timer.TimerTick += Raise.EventWith(this, new EventArgs());
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(outputStr)));
        }
    }
}

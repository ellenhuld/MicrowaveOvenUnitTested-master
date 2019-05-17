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
    public class Step1_CookControllerPowerTube
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
            _timer = Substitute.For<ITimer>();
            _display = Substitute.For<IDisplay>();
            _powerTube = new PowerTube(_output);

            _cookController = new CookController(_timer, _display, _powerTube);
        }

        [Test]
        public void CookController_StartCooking50_Power50()
        {
            _cookController.StartCooking(50, 60);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("50")));
        }



    }
}

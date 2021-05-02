using FluentAssertions;
using Launchpad.Actions;
using Launchpad.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Launchpad.Tests.ViewModels
{
    public class ButtonSettingsViewModelTests
    {
        Mock<IActionFactory> _ActionFactoryMock;
        IAction _Action1;
        IAction _Action2;
        const string PlayASound = "Play a Sound";
        const string RunAProgram = "Run a Program";

        ButtonSettingsViewModel _Sut;
        public ButtonSettingsViewModelTests()
        {
            _Action1 = new PlaySoundAction();
            _Action2 = new RunProgramAction();
            _ActionFactoryMock = new Mock<IActionFactory>();
            _ActionFactoryMock.Setup(x => x.GetAllNames()).Returns(new List<string>() { PlayASound, RunAProgram });

            _Sut = new ButtonSettingsViewModel(_ActionFactoryMock.Object);
            _Sut.SetSettings(new Models.UIButtonSetting());
        }

        [Fact]
        public void Constructor_FillsActionList_WhenCalled()
        {
            _Sut.Actions.Contains(PlayASound).Should().BeTrue();
        }

        [Fact]
        public void SetSelectedAction_CreatesNewInstanceOfSameTypeOnSetting_WhenSet()
        {
            _ActionFactoryMock.Setup(x => x.CreateNewByName(It.IsAny<string>())).Returns(new PlaySoundAction());

            _Sut.SelectedAction = PlayASound;
            var result = _Sut.GetSettings();

            result.Should().NotBeNull();
            result.ActionWhenPressed.Should().BeOfType(_Action1.GetType());
            result.ActionWhenPressed.Should().NotBe(_Action1);
        }

        [Fact]
        public void ClearSettingsCommand_RemovesSelectedActionAndSetsColorToNone_WhenExecuted()
        {
            _Sut.SelectedAction = PlayASound;
            _Sut.Color = Devices.ButtonColor.Red;

            _Sut.ClearSettingsCommand.Execute(null);

            _Sut.SelectedAction.Should().BeNull();
            _Sut.Color.Should().Be(Devices.ButtonColor.None);
        }

        [Fact]
        public void SelectedAction_IsFilledAutomatically_WhenSettingIsSet()
        {
            _Sut.SetSettings(new Models.UIButtonSetting() { ActionWhenPressed = new RunProgramAction() });

            _Sut.SelectedAction.Should().Be(RunAProgram);
        }
    }
}

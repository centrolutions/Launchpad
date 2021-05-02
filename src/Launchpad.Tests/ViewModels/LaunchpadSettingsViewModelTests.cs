using FluentAssertions;
using Launchpad.Actions;
using Launchpad.Devices;
using Launchpad.Models;
using Launchpad.Services;
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
    public class LaunchpadSettingsViewModelTests
    {
        Mock<ILaunchpadDevice> _LaunchpadDeviceMock;
        Mock<IDialogService> _DialogServiceMock;
        Mock<IButtonSettingsStore> _ButtonSettingsStoreMock;
        LaunchpadSettingsViewModel _Sut;

        public LaunchpadSettingsViewModelTests()
        {
            _LaunchpadDeviceMock = new Mock<ILaunchpadDevice>();
            _DialogServiceMock = new Mock<IDialogService>();
            _ButtonSettingsStoreMock = new Mock<IButtonSettingsStore>();
            _ButtonSettingsStoreMock.Setup(x => x.AllSettings).Returns(new List<UIButtonSetting>());

            _Sut = new LaunchpadSettingsViewModel(_LaunchpadDeviceMock.Object, _DialogServiceMock.Object, _ButtonSettingsStoreMock.Object);
        }

        [Fact]
        public void ClosingCommand_ShutsdownDevice_WhenDeviceIsConnected()
        {
            _LaunchpadDeviceMock.Setup(x => x.DeviceAttached).Returns(true);

            _Sut.ClosingCommand.Execute(null);

            _LaunchpadDeviceMock.Verify(x => x.Reset());
        }

        [Fact]
        public void ClearAllSettingsCommand_RemovesAllButtonSettings_WhenUserAnswersYes()
        {
            var settings = new List<UIButtonSetting>() { new UIButtonSetting() };
            _ButtonSettingsStoreMock.Setup(x => x.AllSettings).Returns(settings);
            _DialogServiceMock.Setup(x => x.ShowYesNoDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            _Sut.ClearAllSettingsCommand.Execute(null);

            settings.Should().BeEmpty();
        }

        [Fact]
        public void ClearAllSettingsCommand_DoesNotRemoveSettings_WhenUserAnswersNo()
        {
            var settings = new List<UIButtonSetting>() { new UIButtonSetting() };
            _ButtonSettingsStoreMock.Setup(x => x.AllSettings).Returns(settings);
            _DialogServiceMock.Setup(x => x.ShowYesNoDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            _Sut.ClearAllSettingsCommand.Execute(null);

            settings.Should().NotBeEmpty();
        }

        [Fact]
        public void OpenButtonSettingsFileCommand_ShowsOpenFileDialog_WhenExecuted()
        {
            _Sut.OpenButtonSettingsFileCommand.Execute(null);

            _DialogServiceMock.Verify(x => x.ShowOpenFileDialog(It.IsAny<string>()));
        }

        [Fact]
        public void OpenButtonSettingsFileCommand_OpensAFile_WhenUserChoosesAFile()
        {
            const string fileName = @"C:\mysave.json";
            _DialogServiceMock.Setup(x => x.ShowOpenFileDialog(It.IsAny<string>())).Returns(fileName);
            var buttonSettings = new List<UIButtonSetting>();
            _ButtonSettingsStoreMock.Setup(x => x.AllSettings).Returns(buttonSettings);

            _Sut.OpenButtonSettingsFileCommand.Execute(null);

            _ButtonSettingsStoreMock.Verify(x => x.OpenSettings(fileName));
        }

        [Fact]
        public void SaveButtonSettingsFileCommand_ShowsSaveFileDialog_WhenExecuted()
        {
            _Sut.SaveButtonSettingsFileCommand.Execute(null);

            _DialogServiceMock.Verify(x => x.ShowSaveFileDialog(It.IsAny<string>()));
        }

        [Fact]
        public void SaveButtonSettingsFileCommand_SavesAFile_WhenUserChoosesToSave()
        {
            const string fileName = @"C:\mysave.json";
            _DialogServiceMock.Setup(x => x.ShowSaveFileDialog(It.IsAny<string>())).Returns(fileName);
            
            _Sut.SaveButtonSettingsFileCommand.Execute(null);

            _ButtonSettingsStoreMock.Verify(x => x.SaveCurrentSettings(fileName));
        }

        [Fact]
        public void LaunchpadDeviceButtonPressedEvent_ExecutesActionForGridButton_WhenButtonSettingExists()
        {
            var mockAction = new Mock<IAction>();
            var setting = new UIButtonSetting() { ActionWhenPressed = mockAction.Object };
            _ButtonSettingsStoreMock.Setup(x => x.GetButtonSetting(ButtonType.Grid, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                                    .Returns(setting);

            _LaunchpadDeviceMock.Raise(x => x.ButtonPressed += null, new ButtonPressEventArgs(0, 0));

            mockAction.Verify(x => x.Execute());
        }

        [Fact]
        public void LaunchpadDeviceButtonPressedEvent_ExecutesActionForSidebarButton_WhenButtonSettingExists()
        {
            var mockAction = new Mock<IAction>();
            var setting = new UIButtonSetting() { ActionWhenPressed = mockAction.Object };
            _ButtonSettingsStoreMock.Setup(x => x.GetButtonSetting(ButtonType.Side, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                                    .Returns(setting);

            _LaunchpadDeviceMock.Raise(x => x.ButtonPressed += null, new ButtonPressEventArgs(SideButton.Arm));

            mockAction.Verify(x => x.Execute());
        }

        [Fact]
        public void LaunchpadDeviceButtonPressedEvent_ExecutesActionForToolbarButton_WhenButtonSettingExists()
        {
            var mockAction = new Mock<IAction>();
            var setting = new UIButtonSetting() { ActionWhenPressed = mockAction.Object };
            _ButtonSettingsStoreMock.Setup(x => x.GetButtonSetting(ButtonType.Toolbar, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                                    .Returns(setting);

            _LaunchpadDeviceMock.Raise(x => x.ButtonPressed += null, new ButtonPressEventArgs(ToolbarButton.Down));

            mockAction.Verify(x => x.Execute());
        }

        [Fact]
        public void OpenGridButtonSettingsCommand_ShowsButtonSettings_WhenExecuted()
        {
            var uiButton = new UIGridButton();
            var setting = new UIButtonSetting();
            _ButtonSettingsStoreMock.Setup(x => x.GetButtonSetting(ButtonType.Grid, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(setting);
            var buttonMock = new Mock<ILaunchpadButton>();
            _LaunchpadDeviceMock.Setup(x => x[It.IsAny<int>(), It.IsAny<int>()]).Returns(buttonMock.Object);

            _Sut.OpenGridButtonSettingsCommand.Execute(uiButton);

            _DialogServiceMock.Verify(x => x.ShowButtonSettingsDialog(setting));
        }

        [Fact]
        public void OpenToolbarButtonSettingsCommand_ShowsButtonSettings_WhenExecuted()
        {
            var uiButton = new UIToolbarButton();
            var setting = new UIButtonSetting();
            _ButtonSettingsStoreMock.Setup(x => x.GetButtonSetting(ButtonType.Toolbar, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(setting);
            var buttonMock = new Mock<ILaunchpadButton>();
            _LaunchpadDeviceMock.Setup(x => x.GetButton(It.IsAny<ToolbarButton>())).Returns(buttonMock.Object);

            _Sut.OpenToolbarButtonSettingsCommand.Execute(uiButton);

            _DialogServiceMock.Verify(x => x.ShowButtonSettingsDialog(setting));
        }

        [Fact]
        public void OpenSidebarButtonSettingsCommand_ShowsButtonSettings_WhenExecuted()
        {
            var uiButton = new UISidebarButton();
            var setting = new UIButtonSetting();
            _ButtonSettingsStoreMock.Setup(x => x.GetButtonSetting(ButtonType.Side, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(setting);
            var buttonMock = new Mock<ILaunchpadButton>();
            _LaunchpadDeviceMock.Setup(x => x.GetButton(It.IsAny<SideButton>())).Returns(buttonMock.Object);

            _Sut.OpenSidebarButtonSettingsCommand.Execute(uiButton);

            _DialogServiceMock.Verify(x => x.ShowButtonSettingsDialog(setting));
        }

    }
}

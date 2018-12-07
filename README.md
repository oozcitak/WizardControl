[![License](http://img.shields.io/github/license/oozcitak/wizardcontrol.svg?style=flat-square)](https://opensource.org/licenses/MIT)
[![Nuget](https://img.shields.io/nuget/v/WizardControl.svg?style=flat-square)](https://www.nuget.org/packages/WizardControl)

WizardControl is a .NET control with multiple pages for a wizard user interface. The control is based on [PagedControl](https://github.com/oozcitak/PagedControl).

![WizardControl in Designer](https://raw.githubusercontent.com/wiki/oozcitak/WizardControl/WizardControl.designer.png)

The control has full designer support for adding/removing pages and dragging child controls. It is also possible to programatically add/remove wizard pages by using the `Pages` property of the control.

![WizardControl in Use](https://raw.githubusercontent.com/wiki/oozcitak/WizardControl/WizardControl.in_use.png)

# Installation #

If you are using [NuGet](https://nuget.org/) you can install the assembly with:

`PM> Install-Package WizardControl`

# Properties #

Following public properties are available.

|Name|Type|Description|
|----|----|-----------|
|BackButtonText    |string                        |Gets or sets the text of the `Back` button. This is a localizable property.|
|NextButtonText    |string                        |Gets or sets the text of the `Next` button. This is a localizable property.|
|CloseButtonText   |string                        |Gets or sets the text of the `Close` button. This is a localizable property.|
|HelpButtonText    |string                        |Gets or sets the text of the `Help` button. This is a localizable property.|
|BackButtonEnabled |bool                          |Gets or sets whether the `Back` button is enabled by user code.|
|NextButtonEnabled |bool                          |Gets or sets whether the `Next` button is enabled by user code.|
|CloseButtonEnabled|bool                          |Gets or sets whether the `Close` button is enabled by user code.|
|HelpButtonEnabled |bool                          |Gets or sets whether the `Help` button is enabled by user code.|
|BackButtonVisible |bool                          |Gets or sets whether the `Back` button is visible.|
|NextButtonVisible |bool                          |Gets or sets whether the `Next` button is visible.|
|CloseButtonVisible|bool                          |Gets or sets whether the `Close` button is visible.|
|HelpButtonVisible |bool                          |Gets or sets whether the `Help` button is visible.|
|SelectedPage      |Page                          |Gets or sets the currently selected page.|
|SelectedIndex     |int                           |Gets or sets the index of the currently selected page.|
|Pages             |PagedControl.PageCollection   |Gets the collection of pages.|
|OwnerDraw         |bool                          |Gets or sets whether the pages will be manually drawn by the user.|
|CanGoBack         |bool                          |Gets whether the control can navigate to the previous page.|
|CanGoNext         |bool                          |Gets whether the control can navigate to the next page.|
|DisplayRectangle  |System.Drawing.Rectangle      |Gets the client rectangle where pages are located. Deriving classes can override this property to modify page bounds.|
|UIControls        |System.Windows.Forms.Control[]|Gets the collection of UI controls on the wizard. The control creates the `Back`, `Next`, `Close` and `Help` buttons automatically. Deriving classes can override this method to provide additional UI controls.|

# Methods #

Following public methods are available.

|Name|Description|
|----|-----------|
|GoBack()|Navigates to the previous page if possible.|
|GoNext()|Navigates to the next page if possible.|

# Events #

Following events are raised by the control:

|Name|Event Argument|Description|
|----|--------------|-----------|
|BackButtonClicked |System.ComponentModel.CancelEventArgs|Occurs when the `Back` button of the wizard is clicked by the user. The control switches to the previous page (if any) by default. It is possible to cancel the default page switching behavior by setting `Cancel = true` of the event arguments while handling the event.|
|NextButtonClicked |System.ComponentModel.CancelEventArgs|Occurs when the `Next` button of the wizard is clicked by the user. The control switches to the next page (if any) by default. It is possible to cancel the default page switching behavior by setting `Cancel = true` of the event arguments while handling the event.|
|CloseButtonClicked|System.ComponentModel.CancelEventArgs|Occurs when the `Close` button of the wizard is clicked by the user. The control closes the containing form by default. It is possible to cancel the default behavior by setting `Cancel = true` of the event arguments while handling the event.|
|HelpButtonClicked |System.EventArgs                     |Occurs when the `Help` button of the wizard is clicked by the user.|
|CurrentPageChanging|PagedControl.PageChangingEventArgs|Occurs before the selected page changes. The event arguments contains references to the currently selected page and the page to become selected. It is possible to make the control navigate to a different page by setting the `NewPage` property of the event arguments, or to cancel navigation entirely by setting `Cancel = true` while handling the event.|
|CurrentPageChanged |PagedControl.PageChangedEventArgs |Occurs after the selected page changes. The event arguments contains references to the currently selected page and the previous selected page.|
|PageAdded  |PagedControl.PageEventArgs|Occurs after a new page is added to the page collection. The event arguments contains a reference to the new page.|
|PageRemoved|PagedControl.PageEventArgs|Occurs after an existing page is removed from the page collection. The event arguments contains a reference to the removed page.|
|PageValidating|PagedControl.PageValidatingEventArgs|Occurs before the selected page changes and it needs to be validated. The event arguments contains a reference to the currently selected page. By setting `Cancel = true` while handling the event, the validation stops and the selected page is not changed.|
|PageValidated |PagedControl.PageEventArgs          |Occurs before the selected page changes and after it is successfully validated. The event arguments contains a reference to the currently selected page.|
|PageHidden    |PagedControl.PageEventArgs          |Occurs before the selected page changes and after the currently selected page is hidden. The event arguments contains a reference to the page.|
|PageShown     |PagedControl.PageEventArgs          |Occurs before the selected page changes and the page to become selected is shown. The event arguments contains a reference to the page.|
|PagePaint       |PagedControl.PagePaintEventArgs|Occurs when a page is needed to be painted. The control paints the background of the pages by default. However, if the `OwnerDraw` property of the control is set to `true`, all page painting should be done manually in this event.|
|UpdateUIControls|System.EventArgs               |Occurs when the visual states of the user interface controls are needed to be updated. The control handles the visual states of the `Back`, `Next`, `Close` and `Help` buttons automatically when the selected page changes. If any custom UI controls are added by overriding the `UIControls` property, visual states of those controls should be handled in this event.|

---
uid: Home
title: Home
---
# WizardControl #

The @Manina.Windows.Forms.WizardControl winforms control contains multiple pages for a wizard user interface. The control is based on [PagedControl](https://github.com/oozcitak/PagedControl).

Pages can be added at design time, or at run-time through the `Pages` property of the control. 

The user can navigate between pages by setting the `SelectedPage` and `SelectedIndex` properties. Or calling the `GoBack` and `GoNext` methods for sequental navigation.

When a page is switched, a number of events are fired by the control. Most important of these are the `PageValidating` and the `PageChanging` event. The latter allows the user to change the target page or to cancel the page change entirely.

<div>![WizardControl in Designer](./resources/images/WizardControl.designer.png)</div>


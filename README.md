# blazor-rx-mouse
A proof of concept to handle mouse events in blazor using RX.net

## Introduction
This POC illustrates how mouse event state can be managed using RX.net. The mouse state using a `MouseState` object.

`MouseEventLayer.razor` listens to the events and relays them to the mouse state object. RX.net is used in `Home.razor` 
to update application state based on mouse events.

The mouse state object can be passed into the component hierarchy as cascading state. In testing scenario's, the `MouseState` 
can be updated from the test to simulate mouse interactions.
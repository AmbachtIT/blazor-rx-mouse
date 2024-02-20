using System.Numerics;
using System.Reactive.Subjects;
using System.Reactive;

namespace BlazorRxMouse.Client
{
	public record class MouseState : ReactiveObject
	{

		/// <summary>
		/// Mouse position
		/// </summary>
		public MousePosition Position
		{
			get => _position;
			set => SetField(ref _position, value);
		}
		private MousePosition _position;

		/// <summary>
		/// Is the left button currently pressed?
		/// </summary>
		public bool LeftButtonPressed
		{
			get => _leftButtonPressed;
			set => SetField(ref _leftButtonPressed, value);
		}

		private bool _leftButtonPressed;

		/// <summary>
		/// Is the middle button currently pressed?
		/// </summary>
		public bool MiddleButtonPressed
		{
			get => _middleButtonPressed;
			set => SetField(ref _middleButtonPressed, value);
		}

		private bool _middleButtonPressed;


		/// <summary>
		/// Is the right button currently pressed?
		/// </summary>
		public bool RightButtonPressed
		{
			get => _rightButtonPressed;
			set => SetField(ref _rightButtonPressed, value);
		}

		private bool _rightButtonPressed;

		/// <summary>
		/// Is the cursor currently hovering over the layer intercepting the events
		/// </summary>
		public bool Over
		{
			get => _over;
			set => SetField(ref _over, value);
		}
		private bool _over;

	}
}

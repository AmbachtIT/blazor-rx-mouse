using System.Numerics;

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

		public MouseButtonState Left { get; } = new();
		public MouseButtonState Right { get; } = new();
		public MouseButtonState Middle { get; } = new();


	}
}

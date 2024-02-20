using System.Numerics;

namespace BlazorRxMouse.Client
{
	public record struct MousePosition
	{
		public MousePosition(Vector2 screen, Vector2 page, Vector2 client, Vector2 offset, Vector2 movement)
		{
			this.Screen = screen;
			this.Page = page;
			this.Client = client;
			this.Offset = offset;
			this.Movement = movement;
		}

		/// <summary>
		/// Coordinates of the mouse pointer in global (screen) coordinates
		/// </summary>
		public Vector2 Screen { get; }

		/// <summary>
		/// Coordinates of the mouse pointer relative to the whole document
		/// </summary>
		public Vector2 Page { get; }

		/// <summary>
		/// Coordinates of the mouse pointer in local (DOM content) coordinates
		/// </summary>
		public Vector2 Client { get; }

		/// <summary>
		/// Coordinates of the mouse pointer in relative (target element) coordinates
		/// </summary>
		public Vector2 Offset { get; }

		/// <summary>
		/// Coordinates of the mouse pointer relative to the previous mouse position
		/// </summary>
		public Vector2 Movement { get; }

	}
}

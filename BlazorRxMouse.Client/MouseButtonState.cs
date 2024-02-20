using System.Reactive;
using System.Reactive.Subjects;

namespace BlazorRxMouse.Client
{
	public record class MouseButtonState : ReactiveObject
	{

		/// <summary>
		/// Is the button currently pressed?
		/// </summary>
		public bool Pressed
		{
			get => _pressed;
			set
			{
				if (SetField(ref _pressed, value))
				{
					if (value)
					{
						OnDown.OnNext(Unit.Default);
					}
					else
					{
						OnUp.OnNext(Unit.Default);
					}
				}
			}
		}

		private bool _pressed;

		/// <summary>
		/// Is raised when the button is pressed down
		/// </summary>
		public ISubject<Unit> OnDown { get; } = new Subject<Unit>();

		/// <summary>
		/// Is raised when the button is released
		/// </summary>
		public ISubject<Unit> OnUp { get; } = new Subject<Unit>();



	}
}

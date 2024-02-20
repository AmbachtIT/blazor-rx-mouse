using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive;
using System.Reflection;

namespace BlazorRxMouse.Client
{
	/// <summary>
	/// Extension methods to facilitate using common reactive patterns
	/// </summary>
	/// <remarks>
	/// Source: Somewhere on stackoverflow.
	/// </remarks>
	public static class ReactiveExtensions
	{

		/// <summary>
		/// Returns an observable sequence of the value of a property when <paramref name="source"/> raises <seealso cref="INotifyPropertyChanged.PropertyChanged"/> for the given property.
		/// </summary>
		/// <typeparam name="T">The type of the source object. Type must implement <seealso cref="INotifyPropertyChanged"/>.</typeparam>
		/// <param name="source">The object to observe property changes on.</param>
		/// <param name="property">An expression that describes which property to observe.</param>
		/// <returns>Returns an observable sequence of the property values as they change.</returns>
		public static IObservable<Unit> OnAnyPropertyChanges<T>(this T source)
			where T : INotifyPropertyChanged
		{
			return Observable.Create<Unit>(o =>
			{
				return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
						handler => handler.Invoke,
						h => source.PropertyChanged += h,
						h => source.PropertyChanged -= h)
					.Select(_ => Unit.Default)
					.Subscribe();
			});
		}



		/// <summary>
		/// Returns an observable sequence of the value of a property when <paramref name="source"/> raises <seealso cref="INotifyPropertyChanged.PropertyChanged"/> for the given property.
		/// </summary>
		/// <typeparam name="T">The type of the source object. Type must implement <seealso cref="INotifyPropertyChanged"/>.</typeparam>
		/// <typeparam name="TProperty">The type of the property that is being observed.</typeparam>
		/// <param name="source">The object to observe property changes on.</param>
		/// <param name="property">An expression that describes which property to observe.</param>
		/// <returns>Returns an observable sequence of the property values as they change.</returns>
		public static IObservable<TProperty> OnPropertyChanges<T, TProperty>(this T source, Expression<Func<T, TProperty>> property)
			where T : INotifyPropertyChanged
		{
			return Observable.Create<TProperty>(o =>
			{
				var propertyName = property.GetPropertyInfo().Name;
				var propertySelector = property.Compile();

				return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
						handler => handler.Invoke,
						h => source.PropertyChanged += h,
						h => source.PropertyChanged -= h)
					.Where(e => e.EventArgs.PropertyName == propertyName)
					.Select(e => propertySelector(source))
					.Subscribe(o);
			});
		}

		/// <summary>
		/// Performs an async task time the observable value changes
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="observable"></param>
		/// <param name="task"></param>
		/// <param name="isDisposed"></param>
		public static void OnChangeUntilAsync<TProperty>(this IObservable<TProperty> observable, Func<CancellationToken, Task> task, IObservable<Unit> isDisposed)
		{
			observable
				.DistinctUntilChanged()
				.Select(_ => Observable.FromAsync(task))
				.Switch()
				.TakeUntil(isDisposed)
				.Subscribe();
		}

		/// <summary>
		/// Performs an action any time the 
		/// </summary>
		/// <param name="observable"></param>
		/// <param name="task"></param>
		/// <param name="isDisposed"></param>
		public static void OnNextUntil(this IObservable<Unit> observable, Action task, IObservable<Unit> isDisposed)
		{
			observable
				.Do(_ => task())
				.TakeUntil(isDisposed)
				.Subscribe();
		}


		/// <summary>
		/// Performs an action any time the <c>PropertyChanged</c> event is raised for the specified property
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="observable"></param>
		/// <param name="task"></param>
		/// <param name="isDisposed"></param>
		public static void OnChangeUntil<TProperty>(this IObservable<TProperty> observable, Action<TProperty> task, IObservable<Unit> isDisposed)
		{
			observable
				.DistinctUntilChanged()
				.Do(task)
				.TakeUntil(isDisposed)
				.Subscribe();
		}


		/// <summary>
		/// Performs an action any time the <c>PropertyChanged</c> event is raised for the specified property
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="observable"></param>
		/// <param name="task"></param>
		/// <param name="isDisposed"></param>
		public static void OnNextUntilAsync(this IObservable<Unit> observable, Func<CancellationToken, Task> task, IObservable<Unit> isDisposed)
		{
			observable
				.Select(_ => Observable.FromAsync(task))
				.Switch()
				.TakeUntil(isDisposed)
				.Subscribe();
		}


		/// <summary>
		/// Gets property information for the specified <paramref name="property"/> expression.
		/// </summary>
		/// <typeparam name="TSource">Type of the parameter in the <paramref name="property"/> expression.</typeparam>
		/// <typeparam name="TValue">Type of the property's value.</typeparam>
		/// <param name="property">The expression from which to retrieve the property information.</param>
		/// <returns>Property information for the specified expression.</returns>
		/// <exception cref="ArgumentException">The expression is not understood.</exception>
		public static PropertyInfo GetPropertyInfo<TSource, TValue>(this Expression<Func<TSource, TValue>> property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}

			var body = property.Body as MemberExpression;
			if (body == null)
			{
				throw new ArgumentException("Expression is not a property", "property");
			}

			var propertyInfo = body.Member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException("Expression is not a property", "property");
			}

			return propertyInfo;
		}

	}
}

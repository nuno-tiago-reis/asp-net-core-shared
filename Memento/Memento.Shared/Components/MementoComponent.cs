﻿using AutoMapper;
using JetBrains.Annotations;
using Memento.Shared.Services.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Sotsera.Blazor.Toaster;
using System.Threading.Tasks;

namespace Memento.Shared.Components
{
	/// <summary>
	/// Implements an abstract component that provides logging and mapping properties.
	/// </summary>
	/// 
	/// <seealso cref="ComponentBase" />
	[UsedImplicitly]
	public abstract class MementoComponent<T> : ComponentBase
	{
		#region [Parameters]
		/// <summary>
		/// The authentication state.
		/// </summary>
		[CascadingParameter]
		[UsedImplicitly]
		protected Task<AuthenticationState> AuthenticationState { get; set; }
		#endregion

		#region [Services]
		/// <summary>
		/// The navigation manager.
		/// </summary>
		[Inject]
		[UsedImplicitly]
		protected NavigationManager NavigationManager { get; set; }

		/// <summary>
		/// The logger service.
		/// </summary>
		[Inject]
		[UsedImplicitly]
		protected ILogger<T> Logger { get; set; }

		/// <summary>
		/// The mapper service
		/// </summary>
		[Inject]
		[UsedImplicitly]
		protected IMapper Mapper { get; set; }

		/// <summary>
		/// The localizer service.
		/// </summary>
		[Inject]
		[UsedImplicitly]
		protected ILocalizerService Localizer { get; set; }

		/// <summary>
		/// The toaster service.
		/// </summary>
		[Inject]
		[UsedImplicitly]
		protected IToaster Toaster { get; set; }
		#endregion

		#region [Methods]
		/// <summary>
		/// Checks if the user is authenticated.
		/// </summary>
		[UsedImplicitly]
		public async Task<bool> IsAuthenticated()
		{
			var state = await this.AuthenticationState;

			return state.User.Identity.IsAuthenticated;
		}

		/// <summary>
		/// Checks if the user is an administrator.
		/// </summary>
		[UsedImplicitly]
		public async Task<bool> IsAdministrator()
		{
			var state = await this.AuthenticationState;

			return state.User.IsInRole("Administrator");
		}
		#endregion
	}
}
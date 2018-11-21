using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace SecretSanta.Web.Infrastructure.Activators
{
    public class DelegatingControllerActivator : IControllerActivator
    {
        private readonly Func<ControllerContext, object> controllerCreator;
        private readonly Action<ControllerContext, object> controllerReleaser;

        public DelegatingControllerActivator(Func<ControllerContext, object> controllerCreator,
            Action<ControllerContext, object> controllerReleaser = null)
        {
            this.controllerCreator = controllerCreator ??
                                     throw new ArgumentNullException(nameof(controllerCreator));
            this.controllerReleaser = controllerReleaser ?? ((_, __) => { });
        }

        public object Create(ControllerContext context)
        {
            return this.controllerCreator.Invoke(context);
        }

        public void Release(ControllerContext context, object controller)
        {
            this.controllerReleaser.Invoke(context, controller);
        }
    }
}

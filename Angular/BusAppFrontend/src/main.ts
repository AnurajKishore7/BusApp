import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideNgIconsConfig, provideIcons } from '@ng-icons/core';
import { heroTruck, heroHome, heroUserCircle, heroXMark } from '@ng-icons/heroicons/outline';
import { routes } from './app/app.routes';
import { authInterceptor } from './app/core/interceptors/auth.interceptor'; 
bootstrapApplication(AppComponent, {
  ...appConfig,
  providers: [
    ...appConfig.providers,
    provideHttpClient(withInterceptors([authInterceptor])), 
    provideNgIconsConfig({ size: '1.5em' }),
    provideIcons({ heroTruck, heroHome, heroUserCircle, heroXMark }),
    provideRouter(routes, withComponentInputBinding())
  ]
})
.catch((err) => console.error(err));
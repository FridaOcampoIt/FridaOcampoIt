import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { TranslocoModule, TRANSLOCO_SCOPE } from '@ngneat/transloco';
import { UserComponent } from 'app/layout/common/user/user.component';
import { SharedModule } from 'app/shared/shared.module';

export const loader = ['en','es'].reduce((acc, lang) => {
    acc[lang] = () => import(`./i18n/${lang}.json`);
    return acc;
}, {});

@NgModule({
    declarations: [
        UserComponent
    ],
    imports     : [
        MatButtonModule,
        MatDividerModule,
        MatIconModule,
        MatMenuModule,
        SharedModule,
        TranslocoModule
    ],
    exports     : [
        UserComponent
    ],
    providers: [{
        provide: TRANSLOCO_SCOPE,
        useValue: { scope: 'profile', loader }
    }]
})
export class UserModule
{
}

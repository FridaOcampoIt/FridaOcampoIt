import { MatSnackBarConfig } from '@angular/material';

export function snackBarConfig() {
    const config = new MatSnackBarConfig();

    config.panelClass = ['default-snack'];

    return config;
}
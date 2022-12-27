import { Injectable, Inject } from '@angular/core';
import { Overlay, OverlayConfig, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';

import { OverlayCargaComponent } from '../../overlay-carga/overlay-carga.component';

interface OverlayConf {
    panelClass?: string;
    hasBackdrop?: boolean;
    backdropClass?: string;
}

const DEFAULT_CONF: OverlayConf = {
    hasBackdrop: true,
    backdropClass: 'cdk-overlay-dark-backdrop',
    panelClass: 'Overlay'
}

@Injectable()
export class OverlayService {
    constructor(
        private overlay: Overlay
    ) { }

    open = (config: OverlayConf = {}): OverlayRef => {
        const dialogConfig = { ...DEFAULT_CONF, ...config };

        const overlayRef = this.crearOverlay(dialogConfig);

        const portal = new ComponentPortal(OverlayCargaComponent);

        overlayRef.attach(portal);

        return overlayRef;
    }

    close = (overlayref: OverlayRef): void => {
        overlayref.dispose();
    }

    private crearOverlay = (config: OverlayConfig) => {
        const overlayConf = this.getOverlayConfig(config);
        return this.overlay.create(overlayConf);
    }

    private getOverlayConfig = (config: OverlayConfig): OverlayConfig => {
        const positionStrat = this.overlay.position().global().centerHorizontally().centerVertically();

        const overlayConf = new OverlayConfig({
            hasBackdrop: config.hasBackdrop,
            backdropClass: config.backdropClass,
            panelClass: config.panelClass,
            scrollStrategy: this.overlay.scrollStrategies.noop(),
            positionStrategy: positionStrat
        });

        return overlayConf;
    }
}
import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { MatDialog, MatSnackBarRef, MatTableDataSource } from '@angular/material';
import { filter } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { DataServices } from '../Interfaces/Services/general.service';
import { enviroments } from '../Interfaces/Enviroments/enviroments';
import { LocationService } from '../Interfaces/Services/location.service';

import { DialogoNotificacionesComponent } from '../tracking/dialogo-notificaciones/dialogo-notificaciones.component';
@Component({
  selector: 'app-origin-ciu',
  templateUrl: './origin-ciu.component.html',
  styleUrls: ['./origin-ciu.component.less']
})
export class OriginCiuComponent implements OnInit {

  Login : Storage;

  ciu: string = "";
  ciudad: string = "";
  compania: string = "";
  cosecha: string = "";
  empresa: string = "";
  pais: string = "";
  planta: string = "";
  producto: string = "";
  sector: string = "";
  cosechero: string = "";
  descripcion: string = "";
  puesto: string = "";
  imagen: string = "";

  prueba: string = "";
  prodimg: string = "";
  recipimg: string = "../../assets/img/learn_more.png";
  contactimg: string = "../../assets/img/bloque_contacto.png";
  colorbg: string = "";
  colortxt: string = "";
  colorval: string = "";
  tipotxt: string = "";
  tipotit: string = "";

  altura: string = "";
  ancho: string = "";
  backimg: string = "";

  titulou: string = "";
  titulod: string = "";

  lat: string = "";
  lon: string = "";

  typeProduct: number = 2;

  movimientoId: number = 0;
  responseDocsProductos: any = "";

  classAuxBackground: String = "";

  resultados = {
    ciu: "",
    nombre: "",
    presentacion: "",
    marca: "",
    lote: "",
    caducidad: "",
    gtin: "",
    empresa: "",
    pais: "",
    estado: "",
    ciudad: "",
    planta: "",
    lineaProd: "",
    sector: "",
    rancho: "",
    operador: "",
    cosecheroNombre: "",
    cosecha: "",
    fechaProduccion: "",
    productoImagen: "",
    descripcion: ""
  };

  overlayRef: OverlayRef;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _overlay: OverlayService,
    private dataService: DataServices,
    private snack: MatSnackBar,
    private _locationService: LocationService,
    private _dialog: MatDialog,
  ) {

  }

  locationNavegador: any = {
    latitude: '',
    longitude: ''
  };
  async ngOnInit() {

    this.Login = sessionStorage;
    this.ciu = "";

    let stringerino: string = window.location.href;
    if (stringerino.includes("ciu=")) {
      let clave = stringerino.split("=");
      let resultado: string = clave[1];

      if (resultado.length) {
        this.ciu = resultado;

        this.obtenerMovimiento();
      } else {
        this.openSnack("El QR tiene una liga vacia o no es válido", "Aceptar");
      }
    }
    
    await this.getLocation();
  }

  async getLocation() {
    this._locationService.getPosition().then(pos => {
        this.locationNavegador = {
          latitude: pos.lat,
          longitude: pos.lng
        }
        this.obtenerCoord();
        console.log('Localizacion', this.locationNavegador);
    });
  }


  arrayAlertas: any[] = [];
  contador: number = 0;
  infoFamilia: any;
  mostrarNotificaciones() {

    const dialogRef = this._dialog.open(DialogoNotificacionesComponent, {
      disableClose: false,
      role: 'alertdialog',
      width: '400',
      height: '200',
      minWidth: '350',
      minHeight: '150',
      data: {
        notificacion: this.arrayAlertas[this.contador], familia: this.infoFamilia, codigo: this.ciu, galeria: false
      }
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (this.arrayAlertas.length > (this.contador + 1)) {
        this.contador++;
        this.mostrarNotificaciones();
      } else {
        this.contador = 0;
      }
    })
  }

  obtenerMovimiento() {
    let data = {
      ciu: this.ciu,
      type: 3
    };

    setTimeout(() => {
      if (this.overlayRef === undefined || !this.overlayRef.hasAttached()) {
        this.overlayRef = this._overlay.open();
      }
    }, 1);

    this.dataService.postData<any>("Tracking/SearchLastMovement", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log(data, "Recibido");
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);

        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          if (data["moveId"] == 0) {
            this.openSnack("Este CIU no cuenta con movimientos registrados.", "Aceptar");
            this.obtenerDatos();
          } else {
            this.movimientoId = data["moveId"];
            this.obtenerDatos();
          }
        }


      },
      error => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("error", error);
        if (error.error.hasOwnProperty("messageEsp")) {
        } else {
          console.log(error);
        }
      }
    );

  }

  obtenerDatos() {
    let data = {
      ciu: this.ciu
    };

    setTimeout(() => {
      if (this.overlayRef === undefined || !this.overlayRef.hasAttached()) {
        this.overlayRef = this._overlay.open();
      }
    }, 1);

    this.dataService.postData<any>("Product/searchDataFromCIUS", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log(data, "Recibido");
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);

        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {

          this.resultados = data["product"][0];
          console.log("resu", this.resultados);

          this.arrayAlertas.push(data["product"][0]["listaAlerta"]);
          let aux = { nombre: data["product"][0]["nombre"], modelo: data["product"][0]["nombre"], compania: data["product"][0]["empresa"] };
          this.infoFamilia = aux;
          console.log(this.arrayAlertas, this.infoFamilia,  'AIUDAAA');

          this.mostrarNotificaciones();
          let fechaProd = "";
          if (this.resultados["fechaProduccion"]) {
            fechaProd = `${new Date(this.resultados["fechaProduccion"]).getDate()}/${new Date(this.resultados["fechaProduccion"]).getMonth() + 1}/${new Date(this.resultados["fechaProduccion"]).getFullYear()}`;
          }

          if (this.resultados["operador"] == null) {
            this.resultados["operador"] = "";
          }

          this.resultados["fechaProduccion"] = fechaProd;

          let imageurl: string = this.resultados["productoImagen"];

          if (imageurl != "") {
            let auxirl = imageurl.split("FamiliesImages");

            let origenurl = `${enviroments.urlBase}FamiliesImages${auxirl[1]}`;
            console.log(`origen: ${auxirl}, origenurl: ${origenurl}`);

            this.resultados["productoImagen"] = origenurl;
          }

          let marcca: string = this.resultados["marca"];
          let prodducto: string = this.resultados["nombre"].split(" ")[0];
          let styles = ["Strawberry", "Blueberry", "Blackberry", "Raspberry"]; //["rojo","azul","morado","rosa"]
          let nombre: string = this.resultados["nombre"];
          let presentacion: string = this.resultados["presentacion"];
          let classAuxBackground: string = "";
          var oVid: any = document.getElementById('titt') || {};
          var oEsp: any = document.getElementById('espe') || {};

          if (marcca == "Mainland Farms") {

            if (prodducto == styles[0]) {
              this.typeProduct = 1;
              this.colortxt = "#fff";
              this.colorval = "#fff";
              this.tipotxt = "Sanchezregular";
              this.tipotit = "Lobster";
              this.altura = "100%";
              this.ancho = "100%";
              this.prodimg = "../../assets/img/head_straw.png";
              //this.recipimg = "../../assets/img/recip_straw.png";
              //this.contactimg = "../../assets/img/contact_straw.png";
              this.colorbg = "#b1171d";
              this.titulou = "Product Information";
              this.titulod = "Origin";
              this.classAuxBackground = "Strawberry";
            }
            else if (prodducto == styles[1]) {
              this.typeProduct = 1;
              this.colortxt = "#fff";
              this.colorval = "#fff";
              this.tipotxt = "Sanchezregular";
              this.tipotit = "Lobster";
              this.altura = "100%";
              this.ancho = "100%";
              this.prodimg = "../../assets/img/head_blue.png";
              //this.recipimg = "../../assets/img/recip_blue.png";
              //this.contactimg = "../../assets/img/contact_blue.png";
              this.colorbg = "#0e67a6";
              this.titulou = "Product Information";
              this.titulod = "Origin";
              this.classAuxBackground = "Blueberry";
            }
            else if (prodducto == styles[2]) {
              this.typeProduct = 1;
              this.colortxt = "#fff";
              this.colorval = "#fff";
              this.tipotxt = "Sanchezregular";
              this.tipotit = "Lobster";
              this.altura = "100%";
              this.ancho = "100%";
              this.prodimg = "../../assets/img/head_black.png";
              //this.recipimg = "../../assets/img/recip_black.png";
              //this.contactimg = "../../assets/img/contact_black.png";
              this.colorbg = "#3b2d5a";
              this.titulou = "Product Information";
              this.titulod = "Origin";
              this.classAuxBackground = "Blackberry";
            }
            else if (prodducto == styles[3]) {
              this.typeProduct = 1;
              this.colortxt = "#fff";
              this.colorval = "#fff";
              this.tipotxt = "Sanchezregular";
              this.tipotit = "Lobster";
              this.altura = "100%";
              this.ancho = "100%";
              this.prodimg = "../../assets/img/head_rasp.png";
              //this.recipimg = "../../assets/img/recip_rasp.png";
              //this.contactimg = "../../assets/img/contact_rasp.png";
              this.colorbg = "#b22055";
              this.titulou = "Product Information";
              this.titulod = "Origin";
              this.classAuxBackground = "Raspberry";
            } else {
              this.typeProduct = 0;
              this.colortxt = "#1D1D1B";
              this.colorval = "#00CEB7";
              this.tipotxt = "sansation-regular";
              this.tipotit = "sansation-bold";
              this.prodimg = "resultados.productoImagen";
              this.altura = "40vw";
              this.ancho = "100%";
              this.backimg = "url(../../assets/img/lineas.svg)";
              this.colorbg = "#fff";
              this.titulou = "Datos del Producto";
              this.titulod = "Origen";
              oVid.innerHTML = '<label class="titulo" style="color: #002760;font-size: 9vw;font-family: sansation-bold;">' + nombre + '</label><label class="subtitulo" style="color: #002760;font-size: 6vw;font-family: sansation-regular;">' + presentacion + '</label>';

            }
          } else {
            this.typeProduct = 0;
          }

          this.BusquedaDocsProductos();
        }


      },
      error => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("error", error);
        if (error.error.hasOwnProperty("messageEsp")) {
        } else {
          console.log(error);
        }
      }
    );
  }

  BusquedaDocsProductos() {

    var request = {
      movimientoId: this.movimientoId
    }

    setTimeout(() => {
      if (this.overlayRef === undefined || !this.overlayRef.hasAttached()) {
        this.overlayRef = this._overlay.open();
      }
    }, 1);

    this.dataService.postData<any>("Movimientos/searchDocDetalleProductos", sessionStorage.getItem("token"), request).subscribe(
      data => {

        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);

        try {
          this.responseDocsProductos = data;
          this.resultados.caducidad = this.responseDocsProductos.docDetalleProductoList.filter(x => x.gtin == this.resultados.gtin)[0].fechaCaducidad.replace(/-/gi, '/');

          if (this.resultados["caducidad"]) {
            this.resultados["caducidad"] = `${new Date(this.resultados["caducidad"]).getDate()}/${new Date(this.resultados["caducidad"]).getMonth() + 1}/${new Date(this.resultados["caducidad"]).getFullYear()}`;
          }

          //this.obtenerCoord();
          this.getLocation();
        } catch (error) {
          //this.obtenerCoord();
          this.getLocation();
        }


      },
      error => {

        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }

        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
      }
    );
  }


  obtenerCoord() {
    this.dataService.getCoorde().subscribe(
      data => {
        this.lat = data["latitude"];
        this.lon = data["longitude"];
        this.registroLog();
      },
      error => {
        console.log("error al obtener la latitud y longitud", error);
      }
    )
  }
 
  


  async registroLog() {
    let data = {
      ciu: this.ciu,
      lat: this.locationNavegador ? this.locationNavegador.latitude : this.lat,
      lon: this.locationNavegador ? this.locationNavegador.longitude : this.lon,
      jeson: "",
      tipo: 2 // 1 = Tracking, 2 = Origin
    };
    await this.dataService.postData<any>("Tracking/RegistroLog", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log("Log Registrado ", data);
      },
      error => {
        console.log("Error al registrar el Log ", error);
      }
    );
  }

  tobottom() {
    var secDesc = document.getElementById("sec-descargas");

    secDesc.scrollIntoView({ behavior: "smooth", block: "end", inline: "nearest" });
  }



  //Funcion para abrir el modal del mensaje
  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
      duration: 5000
    })
  }

}

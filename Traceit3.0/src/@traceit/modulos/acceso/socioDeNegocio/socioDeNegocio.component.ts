import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, NgForm, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from '@services/auth.service';
import {SociosDeNegociosService } from'@services/busnessPather.service'
import { SociosDeNegocios } from '@traceit/types/SocioDeNegocio';

@Component({
    selector     : 'auth-sign-in',
    templateUrl  : './socioDeNegocio.component.html',
    encapsulation: ViewEncapsulation.None,
    animations   : fuseAnimations
})
export class AuthSignInComponent implements OnInit
{
    @ViewChild('signInNgForm') signInNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type   : 'success',
        message: ''
    };
    signInForm: UntypedFormGroup;
    showAlert: boolean = false;
    sociosdenegocios:SociosDeNegocios[];
    sociosdenegocios2:SociosDeNegocios;
    formSocio;
    /**
     * Constructor
     */
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _authService: AuthService,
        private _formBuilder: UntypedFormBuilder,
        private _router: Router,
        private SociosDeNegociosService:SociosDeNegociosService,
        private _fb:FormBuilder,
    )
    {
        this.iniciarformulario();
    }
    iniciarformulario(){
        this.formSocio=this._fb.group({
            
            id:[this.sociosdenegocios2? this.sociosdenegocios2.id:""],
            nombre:[this.sociosdenegocios2? this.sociosdenegocios2.nombre:""],
            razonSocial:[this.sociosdenegocios2?this.sociosdenegocios2.razonsocial:""],
            RFC:[this.sociosdenegocios2?this.sociosdenegocios2.rfc:""],
            estatusId:[this.sociosdenegocios2?this.sociosdenegocios2.estatusid:""],
            })
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        // Create the form
        this.signInForm = this._formBuilder.group({
            user     : ['hughes.brian@company.com', [Validators.required]],
            password  : ['admin', Validators.required],
            rememberMe: ['']
        });
    
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

     getUsers(){
        //Utilizamos el servicio inyectado para encontrar los usuarios
        this.SociosDeNegociosService.findAllUsers().subscribe(res=>{
            if (res.status==200) {
                this.sociosdenegocios=(res.data as SociosDeNegocios[]);
            }
        });
        }
        getone(){
            //Utilizamos el servicio inyectado para encontrar los usuarios
            this.SociosDeNegociosService.findAUser(6).subscribe(res=>{
                if (res.status==200) {
                    this.sociosdenegocios2=(res.data as SociosDeNegocios);
                    console.log(this.sociosdenegocios2)
                }
            });
            }
        otroboton(){
            console.log(this.sociosdenegocios)
        }
        onSubmit(customerData) {
            // Process checkout data here
            this.formSocio.reset();
            this.SociosDeNegociosService.sett(customerData).subscribe(res=>{
                if (res.status==200) {
                    this.sociosdenegocios2=(res.data as SociosDeNegocios);
                    
                }
            });
            console.warn('Your order has been submitted', customerData);
            
        }
        borrar(id:number){
            this.SociosDeNegociosService.deleteUser(id).subscribe(res=>{
                if (res.status==200) {
                    this.sociosdenegocios2=(res.data as SociosDeNegocios);
                    
                }
            });
            
        }
        crearnuevo(){
            console.log(this.formSocio.value)
            this.SociosDeNegociosService.sett(this.formSocio.value).subscribe(res=>{
                if (res.status==200) {
                    this.sociosdenegocios2=(res.data as SociosDeNegocios);
                    
                }
            });
        }
        actualizar(){
            this.SociosDeNegociosService.update(this.formSocio.value).subscribe(res=>{
                if (res.status==200) {
                    this.sociosdenegocios2=(res.data as SociosDeNegocios);
                    
                }
            });
        }
        llenado(id:number){
            console.log(id);
            this.SociosDeNegociosService.findAUser(id).subscribe(res=>{
                if (res.status==200) {
                    this.sociosdenegocios2=(res.data as SociosDeNegocios);
                    console.log(this.sociosdenegocios2)
                    this.iniciarformulario();
                }
            });
        }
         
  
}

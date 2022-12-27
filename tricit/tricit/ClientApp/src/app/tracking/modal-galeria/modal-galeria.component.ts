import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef , MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-modal-galeria',
  templateUrl: './modal-galeria.component.html',
  styleUrls: ['./modal-galeria.component.scss']
})
export class ModalGaleriaComponent implements OnInit {
  getRandomInt(max) {
    return Math.floor(Math.random() * max);
  }

  constructor(
    public _dialogRef: MatDialogRef<ModalGaleriaComponent>,
    @Inject(MAT_DIALOG_DATA) public _data: any
  ) { 
    
  }
  numeroRandom: number = 0;
  tipoAlerta: number = 0;
  ngOnInit(): void {

  }

  close() {
    this._dialogRef.close();
  }

}

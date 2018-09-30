import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-custom-modal',
  templateUrl: './custom-modal.component.html',
  styleUrls: ['./custom-modal.component.css']
})
export class CustomModalComponent implements OnInit {

  @Input('modalId') modalId: string = '';
  @Input('title') title: string = '';
  @Output() public onClose: EventEmitter<any> = new EventEmitter();

  closeModal(): void {
    this.onClose.emit();
  }

  constructor() {
  }

  ngOnInit() {
  }

}

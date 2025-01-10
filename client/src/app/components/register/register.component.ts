import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  model: any = {};
  private accountService = inject(AccountService);
  private toastr = inject(ToastrService);

  register() {
    this.accountService.register(this.model).subscribe({
      next: (response) => console.log(response),
      error: (error) => this.toastr.error(error.error),
    });
  }

  cancel() {
    console.log('cancelled');
  }
}
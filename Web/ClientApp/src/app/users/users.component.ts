import {Component, Inject} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {UserDto} from "./userDto";

@Component({
  selector: 'app-users-component',
  templateUrl: './users.component.html'
})
export class UsersComponent {
  public users: UserDto[] = [];
  public busy: boolean;

  public get isBusy() {
    return this.busy;
  }

  constructor(
    httpClient: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
    this.busy = false;
    httpClient.get<UserDto[]>(baseUrl + 'users')
      .subscribe({
        next: result => {
          this.users = result;
        },
        error: err => console.error(err),
        complete: () => {
          this.busy = false;
        }
      });
  }
}

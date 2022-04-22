import {Component, Inject} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {UserDto} from "./userDto";
import {Router} from "@angular/router";

@Component({
  selector: 'app-users-component',
  templateUrl: './users.component.html'
})
export class UsersComponent {
  public users: UserDto[];
  public busy: boolean;

  public get isBusy() {
    return this.busy;
  }

  constructor(
    private httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router
  ) {
    this.users = [];
    this.busy = false;
    this.loadUsers();
  }

  private loadUsers() {
    this.busy = false;
    this.httpClient.get<UserDto[]>(this.baseUrl + 'users')
      .subscribe({
        next: result => {
          this.users = result;
        },
        error: err => {
          console.error(err);
        },
        complete: () => {
          this.busy = false;
        }
      });
  }

  onClick(id: number) {
    this.router.navigate([`/users/${id}`]).then(value => {
      console.info(value);
    });
  }
}

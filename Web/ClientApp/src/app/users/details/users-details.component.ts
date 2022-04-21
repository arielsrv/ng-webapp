import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ActivatedRoute, ParamMap } from "@angular/router";
import { UserDto } from "../userDto";

@Component({
  selector: 'app-users-component',
  templateUrl: './users-details.component.html'
})
export class UsersDetailsComponent implements OnInit {
  public user: UserDto;
  public busy: boolean;

  public get isBusy() {
    return this.busy;
  }

  constructor(
    private route: ActivatedRoute,
    private httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {
    this.busy = false;
    this.user = {} as UserDto;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.loadUser(params);
    })
  }

  private loadUser(params: ParamMap) {
    let id = params.get('id');
    this.busy = false;
    this.httpClient.get<UserDto>(this.baseUrl + 'users/' + id)
      .subscribe({
        next: result => {
          this.user = result;
        },
        error: err => console.error(err),
        complete: () => {
          this.busy = false;
        }
      });
  }
}

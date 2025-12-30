import { ProjectManagementComponent } from './project-management.component';
import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { of, Subject } from 'rxjs';
import { Actions } from '@ngrx/effects';
import { ProjectCode } from '../../../core/models/project.model';

// Jasmine globals for TypeScript
declare var spyOn: any;
declare var jasmine: any;

class MatDialogMock {
  public afterClosedSubject = new Subject<any>();
  open() {
    return {
      afterClosed: () => this.afterClosedSubject,
      componentInstance: {},
      close: () => {},
      updateSize: () => {},
      backdropClick: () => of(),
      keydownEvents: () => of(),
      addPanelClass: () => {},
      removePanelClass: () => {},
      updatePosition: () => {},
      updateScrollStrategy: () => {},
      disableClose: false
    };
  }
}

describe('ProjectManagementComponent', () => {
  let component: ProjectManagementComponent;
  let fixture: ComponentFixture<ProjectManagementComponent>;
  let store: MockStore;
  let dialog: MatDialogMock;
  let snackBar: MatSnackBar & { calls: any[] };
  let router: any;
  let actions$: Subject<any>;

  const initialState = {
    auth: {
      user: { id: 1, fullName: 'Manager', email: 'manager@example.com', role: 'Manager', isActive: true },
      token: null,
      isAuthenticated: true,
      loading: false,
      error: null
    },
    project: {
      ids: [],
      entities: {},
      selectedProjectId: null,
      loading: false,
      error: null
    }
  };

  let snackBarSpy: any;
  beforeEach(async () => {
    actions$ = new Subject();
    snackBarSpy = { calls: [], open: function(...args: any[]) { this.calls.push(args); } };
    await TestBed.configureTestingModule({
      imports: [ProjectManagementComponent],
      providers: [
        provideMockStore({ initialState: JSON.parse(JSON.stringify(initialState)) }),
        { provide: Router, useValue: {
          navigateCalls: [] as any[][],
          navigate: function () { this.navigateCalls.push(Array.from(arguments)); }
        } },
        { provide: Actions, useValue: actions$ }
      ]
    }).overrideProvider(MatDialog, { useValue: new MatDialogMock() })
      .overrideProvider(MatSnackBar, { useValue: snackBarSpy })
      .compileComponents();

    fixture = TestBed.createComponent(ProjectManagementComponent);
    component = fixture.componentInstance;
    store = TestBed.inject(MockStore);
    dialog = TestBed.inject(MatDialog) as any;
    snackBar = TestBed.inject(MatSnackBar) as any;
    router = TestBed.inject(Router) as any;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should dispatch loadProjects on init', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    component.ngOnInit();
    expect(dispatchSpy).toHaveBeenCalledWith(jasmine.objectContaining({ type: '[Project] Load Projects' }));
  });

  it('should open dialog and dispatch createProject on add', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    component.onAddProject();
    dialog.afterClosedSubject.next({ code: 'P1', projectName: 'Test', clientName: 'Client', isBillable: true });
    expect(dispatchSpy).toHaveBeenCalledWith(jasmine.objectContaining({ type: '[Project] Create Project' }));
  });

  it('should not dispatch createProject if dialog is cancelled', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    component.onAddProject();
    dialog.afterClosedSubject.next(undefined);
    expect(dispatchSpy.calls.allArgs().some((args: any[]) => args[0]?.type === '[Project] Create Project')).toBeFalsy();
  });

  it('should open dialog and dispatch updateProject on edit', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    const project: ProjectCode = {
      id: 1,
      code: 'P1',
      projectName: 'Test',
      clientName: 'Client',
      isBillable: true,
      status: 'Active',
      createdAt: new Date(),
      createdBy: 1
    };
    component.onEditProject(project);
    dialog.afterClosedSubject.next({ code: 'P2', projectName: 'Test2', clientName: 'Client2', isBillable: false, status: 'Inactive' });
    expect(dispatchSpy).toHaveBeenCalledWith(jasmine.objectContaining({ type: '[Project] Update Project' }));
  });

  it('should not dispatch updateProject if dialog is cancelled', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    const project: ProjectCode = {
      id: 1,
      code: 'P1',
      projectName: 'Test',
      clientName: 'Client',
      isBillable: true,
      status: 'Active',
      createdAt: new Date(),
      createdBy: 1
    };
    component.onEditProject(project);
    dialog.afterClosedSubject.next(undefined);
    expect(dispatchSpy.calls.allArgs().some((args: any[]) => args[0]?.type === '[Project] Update Project')).toBeFalsy();
  });

  it('should dispatch deleteProject if confirmed', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    spyOn(window, 'confirm').and.returnValue(true);
    const project: ProjectCode = {
      id: 1,
      code: 'P1',
      projectName: 'Test',
      clientName: 'Client',
      isBillable: true,
      status: 'Active',
      createdAt: new Date(),
      createdBy: 1
    };
    component.onDeleteProject(project);
    expect(dispatchSpy).toHaveBeenCalledWith(jasmine.objectContaining({ type: '[Project] Delete Project' }));
  });

  it('should dispatch createProject with isBillable false', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    component.onAddProject();
    dialog.afterClosedSubject.next({ code: 'P2', projectName: 'Test2', clientName: 'Client2', isBillable: false });
    expect(dispatchSpy).toHaveBeenCalledWith(jasmine.objectContaining({ type: '[Project] Create Project' }));
    const dispatched = dispatchSpy.calls.mostRecent().args[0];
    expect(dispatched.request.isBillable).toBeFalsy();
  });

  it('should dispatch updateProject with status Inactive', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    const project: ProjectCode = {
      id: 2,
      code: 'P2',
      projectName: 'Test2',
      clientName: 'Client2',
      isBillable: false,
      status: 'Active',
      createdAt: new Date(),
      createdBy: 1
    };
    component.onEditProject(project);
    dialog.afterClosedSubject.next({ code: 'P2', projectName: 'Test2', clientName: 'Client2', isBillable: false, status: 'Inactive' });
    expect(dispatchSpy).toHaveBeenCalledWith(jasmine.objectContaining({ type: '[Project] Update Project' }));
    const dispatched = dispatchSpy.calls.mostRecent().args[0];
    expect(dispatched.request.status).toBe('Inactive');
  });

  it('should only dispatch once if dialog closes multiple times with valid and invalid data', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    component.onAddProject();
    dialog.afterClosedSubject.next(undefined);
    dialog.afterClosedSubject.next({ code: 'P3', projectName: 'Test3', clientName: 'Client3', isBillable: true });
    dialog.afterClosedSubject.next({});
    expect(dispatchSpy.calls.allArgs().filter((args: any[]) => args[0]?.type === '[Project] Create Project').length).toBe(1);
  });

  it('should show snackBar with correct duration and label for createProjectSuccess', fakeAsync(() => {
    component.ngOnInit();
    actions$.next({ type: '[Project] Create Project Success' });
    tick();
    const call = snackBar.calls.find((args: any[]) => args[0].includes('created'));
    expect(call[1]).toBe('Close');
    expect(call[2].duration).toBe(3000);
  }));

  it('should show snackBar with correct duration and label for updateProjectSuccess', fakeAsync(() => {
    component.ngOnInit();
    actions$.next({ type: '[Project] Update Project Success' });
    tick();
    const call = snackBar.calls.find((args: any[]) => args[0].includes('updated'));
    expect(call[1]).toBe('Close');
    expect(call[2].duration).toBe(3000);
  }));

  it('should show snackBar with correct duration and label for deleteProjectSuccess', fakeAsync(() => {
    component.ngOnInit();
    actions$.next({ type: '[Project] Delete Project Success' });
    tick();
    const call = snackBar.calls.find((args: any[]) => args[0].includes('deleted'));
    expect(call[1]).toBe('Close');
    expect(call[2].duration).toBe(3000);
  }));

  it('should show snackBar with correct duration and label for failure actions', fakeAsync(() => {
    component.ngOnInit();
    actions$.next({ type: '[Project] Create Project Failure', error: 'failX' });
    tick();
    const call = snackBar.calls.find((args: any[]) => args[0].includes('Error: failX'));
    expect(call[1]).toBe('Close');
    expect(call[2].duration).toBe(5000);
  }));

  it('should not dispatch deleteProject if not confirmed', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    spyOn(window, 'confirm').and.returnValue(false);
    const project: ProjectCode = {
      id: 1,
      code: 'P1',
      projectName: 'Test',
      clientName: 'Client',
      isBillable: true,
      status: 'Active',
      createdAt: new Date(),
      createdBy: 1
    };
    component.onDeleteProject(project);
    expect(dispatchSpy.calls.allArgs().some((args: any[]) => args[0]?.type === '[Project] Delete Project')).toBeFalsy();
  });

  it('should dispatch logout and navigate on logout', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    const navigateSpy = spyOn(router, 'navigate');
    component.onLogout();
    expect(dispatchSpy).toHaveBeenCalledWith(jasmine.objectContaining({ type: '[Auth] Logout' }));
    expect(navigateSpy).toHaveBeenCalledWith(['/login']);
  });

  it('should show snackBar on createProjectSuccess', fakeAsync(() => {
    component.ngOnInit();
    actions$.next({ type: '[Project] Create Project Success' });
    tick();
    expect(snackBar.calls.some((args: any[]) => args[0].includes('created'))).toBeTruthy();
  }));

  it('should show snackBar and dispatch loadProjects on updateProjectSuccess', fakeAsync(() => {
    const dispatchSpy = spyOn(store, 'dispatch').and.callThrough();
    component.ngOnInit();
    actions$.next({ type: '[Project] Update Project Success' });
    tick();
    expect(snackBar.calls.some((args: any[]) => args[0].includes('updated'))).toBeTruthy();
    expect(dispatchSpy).toHaveBeenCalledWith(jasmine.objectContaining({ type: '[Project] Load Projects' }));
  }));

  it('should show snackBar on deleteProjectSuccess', fakeAsync(() => {
    component.ngOnInit();
    actions$.next({ type: '[Project] Delete Project Success' });
    tick();
    expect(snackBar.calls.some((args: any[]) => args[0].includes('deleted'))).toBeTruthy();
  }));


  it('should not dispatch createProject if dialog result is missing fields', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    component.onAddProject();
    dialog.afterClosedSubject.next({});
    expect(dispatchSpy.calls.allArgs().some((args: any[]) => args[0]?.type === '[Project] Create Project')).toBeFalsy();
  });

  it('should not dispatch updateProject if dialog result is missing fields', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    const project: ProjectCode = {
      id: 1,
      code: 'P1',
      projectName: 'Test',
      clientName: 'Client',
      isBillable: true,
      status: 'Active',
      createdAt: new Date(),
      createdBy: 1
    };
    component.onEditProject(project);
    dialog.afterClosedSubject.next({});
    expect(dispatchSpy.calls.allArgs().some((args: any[]) => args[0]?.type === '[Project] Update Project')).toBeFalsy();
  });

  it('should not throw if onDeleteProject called with undefined', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    expect(() => component.onDeleteProject(undefined as any)).not.toThrow();
    expect(dispatchSpy).not.toHaveBeenCalled();
  });

  it('should not dispatch if onDeleteProject called with null', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    expect(() => component.onDeleteProject(null as any)).not.toThrow();
    expect(dispatchSpy).not.toHaveBeenCalled();
  });

  it('should handle onLogout called twice', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    const navigateSpy = spyOn(router, 'navigate');
    component.onLogout();
    component.onLogout();
    expect(dispatchSpy.calls.allArgs().filter((args: any[]) => args[0]?.type === '[Auth] Logout').length).toBe(2);
    expect(navigateSpy.calls.allArgs().filter((args: any[]) => args[0][0] === '/login').length).toBe(2);
  });

  it('should show error snackBar on failure actions with no error message', fakeAsync(() => {
    component.ngOnInit();
    actions$.next({ type: '[Project] Create Project Failure' });
    tick();
    expect(snackBar.calls.some((args: any[]) => args[0].includes('Error: undefined'))).toBeTruthy();
  }));

  it('should show error snackBar on failure actions', fakeAsync(() => {
    component.ngOnInit();
    actions$.next({ type: '[Project] Create Project Failure', error: 'fail' });
    tick();
    expect(snackBar.calls.some((args: any[]) => args[0].includes('Error: fail'))).toBeTruthy();
    actions$.next({ type: '[Project] Update Project Failure', error: 'fail2' });
    tick();
    expect(snackBar.calls.some((args: any[]) => args[0].includes('Error: fail2'))).toBeTruthy();
    actions$.next({ type: '[Project] Delete Project Failure', error: 'fail3' });
    tick();
    expect(snackBar.calls.some((args: any[]) => args[0].includes('Error: fail3'))).toBeTruthy();
  }));
});

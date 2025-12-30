
import { ProjectEffects } from './project.effects';
import * as ProjectActions from '../actions/project.actions';
import { ProjectService } from '../../core/services/project.service';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable, of, throwError, Subject } from 'rxjs';
import { TestBed } from '@angular/core/testing';
import { provideMockStore } from '@ngrx/store/testing';
import { ProjectCode } from '../../core/models/project.model';

describe('ProjectEffects', () => {
  let actions$: Subject<any>;
  let effects: ProjectEffects;
  let projectService: any;

  beforeEach(() => {
    projectService = {
      getAllProjects: function() {},
      getProjectById: function() {},
      createProject: function() {},
      updateProject: function() {},
      deactivateProject: function() {}
    };
    actions$ = new Subject();
    TestBed.configureTestingModule({
      providers: [
        ProjectEffects,
        provideMockActions(() => actions$),
        provideMockStore(),
        { provide: ProjectService, useValue: projectService }
      ]
    });
    effects = TestBed.inject(ProjectEffects);
  });

  it('should dispatch loadProjectsSuccess on loadProjects$', () => {
    const projects: ProjectCode[] = [{ id: 1, code: 'P1', projectName: 'Proj', clientName: 'C', isBillable: true, status: 'Active', createdAt: new Date(), createdBy: 1 }];
    projectService.getAllProjects = () => of(projects);
    let emitted;
    effects.loadProjects$.subscribe(result => emitted = result);
    actions$.next(ProjectActions.loadProjects());
    expect(emitted).toEqual(ProjectActions.loadProjectsSuccess({ projects }));
  });

  it('should dispatch loadProjectsFailure on loadProjects$ error', () => {
    projectService.getAllProjects = () => throwError(() => ({ message: 'fail' }));
    let emitted;
    effects.loadProjects$.subscribe(result => emitted = result);
    actions$.next(ProjectActions.loadProjects());
    expect(emitted).toEqual(ProjectActions.loadProjectsFailure({ error: 'fail' }));
  });
});

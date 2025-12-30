import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, catchError, exhaustMap, mergeMap } from 'rxjs/operators';
import { ProjectService } from '../../core/services/project.service';
import * as ProjectActions from '../actions/project.actions';

/**
 * Project effects for handling side effects
 */
@Injectable()
export class ProjectEffects {
  private actions$ = inject(Actions);
  private projectService = inject(ProjectService);

  /**
   * Load all projects effect
   */
  loadProjects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProjectActions.loadProjects),
      exhaustMap(() =>
        this.projectService.getAllProjects().pipe(
          map(projects => ProjectActions.loadProjectsSuccess({ projects })),
          catchError(error =>
            of(ProjectActions.loadProjectsFailure({ error: error.message }))
          )
        )
      )
    )
  );

  /**
   * Load single project effect
   */
  loadProject$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProjectActions.loadProject),
      mergeMap(action =>
        this.projectService.getProjectById(action.id).pipe(
          map(project => ProjectActions.loadProjectSuccess({ project })),
          catchError(error =>
            of(ProjectActions.loadProjectFailure({ error: error.message }))
          )
        )
      )
    )
  );

  /**
   * Create project effect
   */
  createProject$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProjectActions.createProject),
      exhaustMap(action =>
        this.projectService.createProject(action.request).pipe(
          map(project => ProjectActions.createProjectSuccess({ project })),
          catchError(error =>
            of(ProjectActions.createProjectFailure({ error: error.message }))
          )
        )
      )
    )
  );

  /**
   * Update project effect
   */
  updateProject$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProjectActions.updateProject),
      exhaustMap(action =>
        this.projectService.updateProject(action.id, action.request).pipe(
          map(project => ProjectActions.updateProjectSuccess({ project })),
          catchError(error =>
            of(ProjectActions.updateProjectFailure({ error: error.message }))
          )
        )
      )
    )
  );

  /**
   * Delete project effect
   */
  deleteProject$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProjectActions.deleteProject),
      exhaustMap(action =>
        this.projectService.deactivateProject(action.id).pipe(
          map(() => ProjectActions.deleteProjectSuccess({ id: action.id })),
          catchError(error =>
            of(ProjectActions.deleteProjectFailure({ error: error.message }))
          )
        )
      )
    )
  );

  constructor() {}
}

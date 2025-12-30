import { createAction, props } from '@ngrx/store';
import { ProjectCode, CreateProjectRequest, UpdateProjectRequest } from '../../core/models/project.model';

/**
 * Project management actions for NgRx store
 */

// Load projects
export const loadProjects = createAction('[Project] Load Projects');

export const loadProjectsSuccess = createAction(
  '[Project] Load Projects Success',
  props<{ projects: ProjectCode[] }>()
);

export const loadProjectsFailure = createAction(
  '[Project] Load Projects Failure',
  props<{ error: string }>()
);

// Load single project
export const loadProject = createAction(
  '[Project] Load Project',
  props<{ id: number }>()
);

export const loadProjectSuccess = createAction(
  '[Project] Load Project Success',
  props<{ project: ProjectCode }>()
);

export const loadProjectFailure = createAction(
  '[Project] Load Project Failure',
  props<{ error: string }>()
);

// Create project
export const createProject = createAction(
  '[Project] Create Project',
  props<{ request: CreateProjectRequest }>()
);

export const createProjectSuccess = createAction(
  '[Project] Create Project Success',
  props<{ project: ProjectCode }>()
);

export const createProjectFailure = createAction(
  '[Project] Create Project Failure',
  props<{ error: string }>()
);

// Update project
export const updateProject = createAction(
  '[Project] Update Project',
  props<{ id: number; request: UpdateProjectRequest }>()
);

export const updateProjectSuccess = createAction(
  '[Project] Update Project Success',
  props<{ project: ProjectCode }>()
);

export const updateProjectFailure = createAction(
  '[Project] Update Project Failure',
  props<{ error: string }>()
);

// Delete project
export const deleteProject = createAction(
  '[Project] Delete Project',
  props<{ id: number }>()
);

export const deleteProjectSuccess = createAction(
  '[Project] Delete Project Success',
  props<{ id: number }>()
);

export const deleteProjectFailure = createAction(
  '[Project] Delete Project Failure',
  props<{ error: string }>()
);

export interface ServerResponse<T = null> {
  data: T
  success: boolean
  errors: string[];
}

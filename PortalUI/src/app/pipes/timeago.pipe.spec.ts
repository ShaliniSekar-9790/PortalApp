import { TimeagoPipe } from '../pipes/timeago.pipe';

describe('TimeagoPipe', () => {
  it('create an instance', () => {
    const pipe = new TimeagoPipe();
    expect(pipe).toBeTruthy();
  });
});

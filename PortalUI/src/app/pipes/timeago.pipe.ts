import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeago'
})
export class TimeagoPipe implements PipeTransform {

  transform(value: string, ...args: unknown[]): string {
    var timeago : string = "";
     let monthsago = "days ago";
    let monthago = "a day ago";
    let daysago = "days ago";
    let dayago = "a day ago";
    let hoursago = "hours ago";
    let hourago = "hour ago";
    let minsago = "mins ago";
    let minago = " min ago";
    let secsago = "secs ago";
    let secago = "sec ago";

    var createDate = new Date(value);
    var currentDate = new Date();

    //setting month
    let month =  Math.floor(currentDate.getMonth() - createDate.getMonth());
    timeago = this.formatAgoTime(month, monthsago, monthago);
    if(month > 0) return timeago;

    //setting day
    let days =  Math.floor(currentDate.getDay() - createDate.getDay());
    timeago = this.formatAgoTime(days, daysago, dayago);
    if(days > 0) return timeago;

    //setting hour
    let hour =  Math.floor(currentDate.getHours() - createDate.getHours());
    timeago =  this.formatAgoTime(hour, hoursago, hourago);
    if(hour > 0) return timeago;


    //setting minute
    let min =  Math.floor(currentDate.getMinutes() - createDate.getMinutes());
    timeago = this.formatAgoTime(min, minsago, minago);
    if(min > 0) return timeago;

    //setting seconds
    let sec =  Math.floor(currentDate.getSeconds() - createDate.getSeconds());
    timeago = this.formatAgoTime(sec, secsago, secago);
    if(sec > 0) return timeago;


    return timeago;
  }



  formatAgoTime(diff: number, timesagoStr: string, timeagoStr: string) : string {
    var time = '';
    if(diff == 1) return diff + "  " + timeagoStr; 
    if(diff > 1) return diff + "  " + timesagoStr;
    return time; 
  }
}

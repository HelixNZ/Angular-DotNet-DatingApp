import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {
  photos: Photo[];

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles() {
    this.adminService.getPhotosForApproval().subscribe(photos => {
      this.photos = photos;
    });
  }

  approvePhoto(photoId: number) {
    this.adminService.updatePhotoStatus(photoId, true).subscribe(() => {
      this.photos.splice(this.photos.findIndex(p => p.id === photoId), 1);
    });
  }

  rejectPhoto(photoId: number) {
    this.adminService.updatePhotoStatus(photoId, false).subscribe(() => {
      this.photos.splice(this.photos.findIndex(p => p.id === photoId), 1);
    });
  }
}

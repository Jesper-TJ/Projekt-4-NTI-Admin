<template>
    <v-row justify="center">
      <v-col cols="12" md="8" lg="6">
        <div v-if="data === undefined" class="loading-message">
          Loading...
        </div>
        <div v-else-if="data.computer" class="computer-details">
          <v-card class="computer-card">
            <div class="details-container">
              <h2 class="text-h5 computer-title">{{ data.computer.name }}</h2>
              <v-divider class="my-4" />
              <p class="text-body-1 info">
                <strong>Serial:</strong> {{ data.computer.serial }}
              </p>
              <p class="text-body-1 info">
                <strong>Damage:</strong> {{ data.computer.damage }}
              </p>
              <p class="text-body-1 info">
                <strong>Entry Date:</strong> {{ data.computer.entry_date }}
              </p>
            </div>
          </v-card>
  
          <v-card class="status-card">
            <div class="details-container">
              <h2 class="text-h5 status-title">Computer Status</h2>
              <v-divider class="my-4" />
              <p
                class="status-text"
                :class="{
                  'red-status': data.computer.status === 'Overdue',
                  'orange-status': data.computer.status === 'In Use',
                  'green-status': data.computer.status === 'Returned'
                }"
              >
                {{ data.computer.status }}
              </p>
              <p class="text-body-1 info">
                <strong>Return Date:</strong> {{ data.computerlog.return_date }}
              </p>
            </div>
          </v-card>
        </div>
      </v-col>
    </v-row>
  </template>
  
  <script lang="ts" setup>
  import { IComputer, IComputerlog } from "@/types/ComputerTypes";
  
  defineProps<{
    data?: {
      computer: IComputer;
      computerlog: IComputerlog;
    };
  }>();
  </script>
  
  <style scoped>
  /* General styles */
  .computer-card,
  .status-card {
    background-color: #222831;
    color: #eeeeee;
    border-radius: 12px;
    box-shadow: 0 6px 20px rgba(0, 0, 0, 0.7);
    padding: 20px;
    overflow: hidden;
    transition: transform 0.3s ease, box-shadow 0.3s ease;
    margin-bottom: 20px;
  }
  
  .computer-card:hover,
  .status-card:hover {
    transform: scale(1.02);
    box-shadow: 0px 6px 15px rgba(0, 0, 0, 0.5);
  }
  
  /* Titles */
  .computer-title,
  .status-title {
    font-size: 1.5rem;
    font-weight: bold;
    color: #00adb5;
  }
  
  /* Info */
  .info {
    margin-top: 10px;
    line-height: 1.6;
    color: #cfcfcf;
  }
  
  /* Status text */
  .status-text {
    margin-top: 15px;
    padding: 10px;
    border-radius: 6px;
    font-weight: bold;
    text-align: center;
    color: #eeeeee;
  }
  
  .red-status {
    background-color: #ff5722;
  }
  
  .orange-status {
    background-color: #ff9800;
  }
  
  .green-status {
    background-color: #4caf50;
  }
  
  /* Loading message */
  .loading-message {
    color: #bbbbbb;
    text-align: center;
  }
  
  /* Divider */
  .v-divider {
    border-color: #444444;
  }
  
  /* Responsiveness */
  .details-container {
    padding: 15px;
  }
  </style>
  
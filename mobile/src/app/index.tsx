import { StyleSheet, Text, View } from 'react-native';
import { Redirect } from 'expo-router';

export default function IndexPage() {
  // Automatically forwards the user straight to the /plots screen on app launch
  return <Redirect href="/plots" />;
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 24,
    backgroundColor: '#111411', // Matches the image background
    justifyContent: 'center',
  },
  headerText: {
    color: '#00E639', // Accent neon green
    fontSize: 12,
    letterSpacing: 1,
    fontWeight: '600',
    marginBottom: 4,
  },
  titleText: {
    color: '#E0E7E0', // Off-white for crisp, readable title text
    fontSize: 28,
    fontWeight: 'bold',
    marginBottom: 8,
  },
  subText: {
    color: '#607060', // Muted green/gray for secondary information
    fontSize: 16,
  },
});
